using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Services
{
    internal class DockerContainersReconciliator(ILogger<DockerContainersReconciliator> logger, DockerClient dockerClient, IServiceScopeFactory serviceScopeFactory)
    {
        private readonly ILogger<DockerContainersReconciliator> _logger = logger;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public async Task ReconciliateAsync(CancellationToken cancellationToken) 
        {
            var scope = _serviceScopeFactory.CreateAsyncScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var dockerContainers = await _dockerClient.Containers.ListContainersAsync(
                new ContainersListParameters { All = true }, cancellationToken);
            var dbContainersIds = (await dbContext.Containers
                .Select(c => c.Id)
                .ToListAsync(cancellationToken))
                .ToHashSet();

            var addedContainersCount = await AddMissingContainersAsync(dbContext, dockerContainers, dbContainersIds, cancellationToken);
            var removedContainersCount = await RemoveStaleContainersAsync(dbContext, dockerContainers, dbContainersIds, cancellationToken);

            if(addedContainersCount == 0 && removedContainersCount == 0)
            {
                _logger.LogInformation("No containers changes detected.");
                await transaction.RollbackAsync(cancellationToken);
                return;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Reconciling {TotalAffectedContainers} containers. adding {Added}, removing {Removed}.", 
                addedContainersCount + removedContainersCount, 
                addedContainersCount, 
                removedContainersCount);
            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Reconciliation complete. {Added} added, {Removed} removed.",
                addedContainersCount,
                removedContainersCount);
        }

        /// <summary>
        /// Inserts containers that are missing from the DB.
        /// </summary>
        /// <returns>The number of added containers</returns>
        private async Task<int> AddMissingContainersAsync(AppDbContext dbContext, IList<ContainerListResponse> dockerContainers, HashSet<string> dbContainersIds, CancellationToken cancellationToken)
        {
            var added = 0;

            foreach (var dockerContainer in dockerContainers)
            {
                if (!dbContainersIds.Contains(dockerContainer.ID))
                {
                    var result = Container.Create(dockerContainer.ID, []);
                    if (result.IsError)
                        throw new InvalidOperationException(string.Join("\n", result.Errors.Select(e => $"Code: {e.Code}, Description: {e.Description}")));

                    dbContext.Containers.Add(result.Value);
                    added++;
                }
            }

            return added;
        }

        /// <summary>
        /// Removes stale containers that are deleted from the DB.
        /// </summary>
        /// <returns>The number of deleted stale containers</returns>
        private async Task<int> RemoveStaleContainersAsync(AppDbContext dbContext, IList<ContainerListResponse> dockerContainers, HashSet<string> dbContainersIds, CancellationToken cancellationToken)
        {
            var dockerContainersIds = dockerContainers.Select(c => c.ID).ToHashSet();
            var staleIds = dbContainersIds.Except(dockerContainersIds).ToList();

            if (staleIds.Count > 0)
            {
                var stale = await dbContext.Containers
                    .Where(c => staleIds.Contains(c.Id))
                    .ToListAsync(cancellationToken);

                dbContext.Containers.RemoveRange(stale);
            }

            return staleIds.Count;
        }
    }
}
