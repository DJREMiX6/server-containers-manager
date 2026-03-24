using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Containers;
using ServerContainerManager.Domain.Entities.Containers.ValueObjects;
using ContainerState = ServerContainerManager.Domain.Entities.Containers.Enums.ContainerState;
using Port = ServerContainerManager.Domain.Entities.Containers.ValueObjects.Port;

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
            var updatedContainersCount = await UpdateContainersAsync(dbContext, dockerContainers, dbContainersIds, cancellationToken);

            if (addedContainersCount == 0 && removedContainersCount == 0)
            {
                _logger.LogInformation("No containers changes detected.");
                await transaction.RollbackAsync(cancellationToken);
                return;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Reconciling {TotalAffectedContainers} containers. adding {Added}, updating {Updated}, removing {Removed}.", 
                addedContainersCount + removedContainersCount, 
                addedContainersCount,
                updatedContainersCount,
                removedContainersCount);
            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Reconciliation complete. {Added} added, {Updated} updated, {Removed} removed.",
                addedContainersCount,
                updatedContainersCount,
                removedContainersCount);
        }

        /// <summary>
        /// Inserts containers that are missing, into the DB.
        /// </summary>
        /// <returns>The number of added containers</returns>
        private static async Task<int> AddMissingContainersAsync(AppDbContext dbContext, IList<ContainerListResponse> dockerContainers, HashSet<string> dbContainersIds, CancellationToken cancellationToken)
        {
            var added = 0;

            foreach (var dockerContainer in dockerContainers)
            {
                if (dbContainersIds.Contains(dockerContainer.ID))
                    continue;

                var portsResults = dockerContainer.Ports.Select(p => Port.Create(p.PublicPort, p.PrivatePort)).ToList();
                if (portsResults.Any(p => p.IsError))
                    throw new InvalidOperationException(string.Join('\n', portsResults.Where(p => p.IsError).SelectMany(e => e.Errors).Select(e => $"Code: {e.Code}, Description: {e.Description}.")));

                var labelsResults = dockerContainer.Labels.Select((kv) => Label.Create(kv.Key, kv.Value));
                if (labelsResults.Any(l => l.IsError))
                    throw new InvalidOperationException(string.Join('\n', labelsResults.Where(l => l.IsError).SelectMany(e => e.Errors).Select(e => $"Code: {e.Code}, Description: {e.Description}.")));

                var result = Container.Create(
                    dockerContainer.ID,
                    dockerContainer.Names[0],
                    Enum.Parse<ContainerState>(dockerContainer.State, ignoreCase: true),
                    dockerContainer.Created,
                    [.. labelsResults.Select(l => l.Value)],
                    [.. portsResults.Select(p => p.Value)],
                    []);
                if (result.IsError)
                    throw new InvalidOperationException(string.Join("\n", result.Errors.Select(e => $"Code: {e.Code}, Description: {e.Description}")));

                dbContext.Containers.Add(result.Value);
                added++;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return added;
        }

        /// <summary>
        /// Updates containers that mismatch, with the DB.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dockerContainers"></param>
        /// <param name="dbContainersIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of containers updated.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static async Task<int> UpdateContainersAsync(AppDbContext dbContext, IList<ContainerListResponse> dockerContainers, HashSet<string> dbContainersIds, CancellationToken cancellationToken)
        {
            var updated = 0;

            foreach(var dockerContainer in dockerContainers)
            {
                if (!dbContainersIds.Contains(dockerContainer.ID))
                    continue;

                var container = await dbContext.Containers.FindAsync([dockerContainer.ID], cancellationToken);
                if (container == null)
                    throw new InvalidOperationException($"Cannot find container {dockerContainer.ID} to update.");

                if (container.Name != dockerContainer.Names[0].Trim()[..1]){
                    var renameResult = container.Rename(dockerContainer.Names[0]);
                    if (renameResult.IsError)
                        throw new InvalidOperationException(string.Join('\n', renameResult.Errors.Select(e => $"Code: {e.Code} Description: {e.Description}")));
                }

                var containerState = Enum.Parse<ContainerState>(dockerContainer.State, ignoreCase: true);
                if (container.State != containerState) {
                    var updateStateResult = container.UpdateState(containerState);
                    if (updateStateResult.IsError)
                        throw new InvalidOperationException(string.Join('\n', updateStateResult.Errors.Select(e => $"Code: {e.Code} Description: {e.Description}")));
                }

                var labelsResults = dockerContainer.Labels.Select((kv) => Label.Create(kv.Key, kv.Value));
                if (labelsResults.Any(l => l.IsError))
                    throw new InvalidOperationException(string.Join('\n', labelsResults.Where(l => l.IsError).SelectMany(e => e.Errors).Select(e => $"Code: {e.Code}, Description: {e.Description}.")));
                var labels = labelsResults.Select(l => l.Value).ToList();
                if (!container.Labels.ContentEquals(labels)){
                    var updateLabelsResult = container.UpdateLabels(labels);
                    if (updateLabelsResult.IsError)
                        throw new InvalidOperationException(string.Join('\n', updateLabelsResult.Errors.Select(e => $"Code: {e.Code} Description: {e.Description}")));
                }

                var ports = dockerContainer.Ports ?? []; // Workaround for bug
                var portsResults = ports.Select(p => Port.Create(p.PublicPort, p.PrivatePort));
                if (portsResults.Any(p => p.IsError))
                    throw new InvalidOperationException(string.Join('\n', portsResults.Where(p => p.IsError).SelectMany(e => e.Errors).Select(e => $"Code: {e.Code}, Description: {e.Description}.")));
                var containerPorts = portsResults.Select(p => p.Value).ToList();
                if (!container.Ports.ContentEquals(containerPorts)){
                    var updatePublicPortsResult = container.UpdatePorts(containerPorts);
                    if (updatePublicPortsResult.IsError)
                        throw new InvalidOperationException(string.Join('\n', updatePublicPortsResult.Errors.Select(e => $"Code: {e.Code} Description: {e.Description}")));
                }

                updated++;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return updated;
        }

        /// <summary>
        /// Removes stale containers that are deleted, from the DB.
        /// </summary>
        /// <returns>The number of deleted stale containers</returns>
        private static async Task<int> RemoveStaleContainersAsync(AppDbContext dbContext, IList<ContainerListResponse> dockerContainers, HashSet<string> dbContainersIds, CancellationToken cancellationToken)
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

            await dbContext.SaveChangesAsync(cancellationToken);

            return staleIds.Count;
        }
    }
}
