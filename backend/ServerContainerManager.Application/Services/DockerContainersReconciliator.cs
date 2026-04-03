using Docker.DotNet;
using Docker.DotNet.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Containers;
using ServerContainerManager.Domain.Entities.Containers.ValueObjects;
using ServerContainerManager.Shared.Utils;
using ServerContainerManager.Shared.Utils.Extensions;
using Actor = ServerContainerManager.Shared.Utils.Actor;
using ContainerState = ServerContainerManager.Domain.Entities.Containers.Enums.ContainerState;
using Port = ServerContainerManager.Domain.Entities.Containers.ValueObjects.Port;

namespace ServerContainerManager.Application.Services
{
    internal class DockerContainersReconciliator(
        ILogger<DockerContainersReconciliator> logger,
        DockerClient dockerClient,
        IServiceScopeFactory serviceScopeFactory,
        TimeProvider timeProvider)
    {
        private readonly ILogger<DockerContainersReconciliator> _logger = logger;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly TimeProvider _timeProvider = timeProvider;

        public async Task ReconciliateAsync(CancellationToken cancellationToken) 
        {
            var actor = Actor.System();
            var now = _timeProvider.GetUtcDateTimeNow();
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var dockerContainers = await _dockerClient.Containers.ListContainersAsync(
                new ContainersListParameters { All = true }, cancellationToken);
            var dockerContainersById = dockerContainers.ToDictionary(c => c.ID);
            var dbContainersIds = (await dbContext.Containers
                .Select(c => c.Id)
                .ToListAsync(cancellationToken))
                .ToHashSet();

            var addedContainersCount = AddMissingContainers(dbContext, dockerContainers, dbContainersIds, actor, now);
            var removedContainersCount = await RemoveStaleContainersAsync(dbContext, dockerContainersById, dbContainersIds, cancellationToken);
            var updatedContainersCount = await UpdateContainersAsync(dbContext, dockerContainersById, dbContainersIds, actor, now, cancellationToken);

            if (addedContainersCount == 0 && removedContainersCount == 0 && updatedContainersCount == 0)
            {
                _logger.LogInformation("No containers changes detected.");
                await transaction.RollbackAsync(cancellationToken);
                return;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Reconciling {TotalAffectedContainers} containers. adding {Added}, updating {Updated}, removing {Removed}.", 
                addedContainersCount + removedContainersCount + updatedContainersCount, 
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
        private static int AddMissingContainers(
            AppDbContext dbContext,
            IList<ContainerListResponse> dockerContainers,
            HashSet<string> dbContainersIds,
            Actor actor,
            DateTime now)
        {
            var added = 0;

            foreach (var dockerContainer in dockerContainers)
            {
                if (dbContainersIds.Contains(dockerContainer.ID))
                    continue;

                var ports = ParsePorts(dockerContainer);
                var labels = ParseLabels(dockerContainer);

                var result = Container.Create(
                    dockerContainer.ID,
                    dockerContainer.Names[0],
                    Enum.Parse<ContainerState>(dockerContainer.State, ignoreCase: true),
                    labels,
                    ports,
                    [], 
                    actor, 
                    now, 
                    dockerContainer.Created);
                ThrowIfError(result);

                dbContext.Containers.Add(result.Value);
                added++;
            }

            return added;
        }

        /// <summary>
        /// Updates containers that mismatch, with the DB.
        /// </summary>
        /// <returns>The number of containers updated.</returns>
        private static async Task<int> UpdateContainersAsync(
            AppDbContext dbContext,
            Dictionary<string, ContainerListResponse> dockerContainersById,
            HashSet<string> dbContainersIds,
            Actor actor,
            DateTime now,
            CancellationToken cancellationToken)
        {
            var idsToUpdate = dbContainersIds.Where(dockerContainersById.ContainsKey).ToList();
            if (idsToUpdate.Count == 0)
                return 0;

            var dbContainers = await dbContext.Containers
                .Where(c => idsToUpdate.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, cancellationToken);

            var updated = 0;

            foreach (var id in idsToUpdate)
            {
                var dockerContainer = dockerContainersById[id];
                var container = dbContainers[id];
                var changesHappened = false;

                if (container.Name != dockerContainer.Names[0].Trim().TrimStart('/'))
                {
                    ThrowIfError(container.Rename(dockerContainer.Names[0], actor, now));
                    changesHappened = true;
                }

                var containerState = Enum.Parse<ContainerState>(dockerContainer.State, ignoreCase: true);
                if (container.State != containerState)
                {
                    ThrowIfError(container.UpdateState(containerState, actor, now));
                    changesHappened = true;
                }

                var labels = ParseLabels(dockerContainer);
                if (!container.Labels.ContentEquals(labels))
                {
                    ThrowIfError(container.UpdateLabels(labels, actor, now));
                    changesHappened = true;
                }

                var ports = ParsePorts(dockerContainer);
                if (!container.Ports.ContentEquals(ports))
                {
                    ThrowIfError(container.UpdatePorts(ports, actor, now));
                    changesHappened = true;
                }

                if (changesHappened)
                    updated++;
            }

            return updated;
        }

        /// <summary>
        /// Removes stale containers that are deleted, from the DB.
        /// </summary>
        /// <returns>The number of deleted stale containers</returns>
        private static async Task<int> RemoveStaleContainersAsync(
            AppDbContext dbContext,
            Dictionary<string, ContainerListResponse> dockerContainersById,
            HashSet<string> dbContainersIds,
            CancellationToken cancellationToken)
        {
            var staleIds = dbContainersIds.Where(id => !dockerContainersById.ContainsKey(id)).ToList();

            if (staleIds.Count > 0)
            {
                await dbContext.Containers
                    .Where(c => staleIds.Contains(c.Id))
                    .ExecuteDeleteAsync(cancellationToken);
            }

            return staleIds.Count;
        }

        private static List<Port> ParsePorts(ContainerListResponse dockerContainer)
        {
            var ports = dockerContainer.Ports ?? [];
            var results = ports.Select(p => Port.Create(p.PublicPort, p.PrivatePort)).ToList();
            ThrowIfAnyError(results);
            return results.Select(r => r.Value).ToList();
        }

        private static List<Label> ParseLabels(ContainerListResponse dockerContainer)
        {
            var results = dockerContainer.Labels.Select(kv => Label.Create(kv.Key, kv.Value)).ToList();
            ThrowIfAnyError(results);
            return results.Select(r => r.Value).ToList();
        }

        private static void ThrowIfError<T>(ErrorOr<T> result)
        {
            if (result.IsError)
                throw new InvalidOperationException(
                    string.Join('\n', result.Errors.Select(e => $"Code: {e.Code}, Description: {e.Description}.")));
        }

        private static void ThrowIfAnyError<T>(List<ErrorOr<T>> results)
        {
            if (results.Any(r => r.IsError))
                throw new InvalidOperationException(
                    string.Join('\n', results.Where(r => r.IsError).SelectMany(e => e.Errors).Select(e => $"Code: {e.Code}, Description: {e.Description}.")));
        }
    }
}
