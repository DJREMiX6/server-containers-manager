using Docker.DotNet;
using Docker.DotNet.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Services
{
    internal class ContainerSyncService(ILogger<ContainerSyncService> logger, DockerClient dockerClient, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<ContainerSyncService> _logger = logger;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ReconcileAsync(stoppingToken).ConfigureAwait(false);
        }

        private async Task ReconcileAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Reconciling all containers with Docker.");

            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var containers = await _dockerClient.Containers.ListContainersAsync(
                new ContainersListParameters { All = true }, stoppingToken);
            var dockerIds = containers.Select(c => c.ID).ToHashSet();
            var existingIds = (await db.Containers
                .Select(c => c.Id)
                .ToListAsync(stoppingToken))
                .ToHashSet();

            var added = 0;

            foreach (var dockerContainer in containers)
            {
                if (!existingIds.Contains(dockerContainer.ID))
                {
                    var result = Container.Create(dockerContainer.ID, []);
                    if (result.IsError)
                        throw new InvalidOperationException(string.Join("\n", result.Errors.Select(e => $"Code: {e.Code}, Description: {e.Description}")));

                    db.Containers.Add(result.Value);
                    added++;
                }
            }

            var staleIds = existingIds.Except(dockerIds).ToList();

            if (staleIds.Count > 0)
            {
                var stale = await db.Containers
                    .Where(c => staleIds.Contains(c.Id))
                    .ToListAsync(stoppingToken);

                db.Containers.RemoveRange(stale);
            }

            await db.SaveChangesAsync(stoppingToken);

            logger.LogInformation(
                "Reconciliation complete. {Added} added, {Removed} removed.",
                added,
                staleIds.Count);
        }
    }
}
