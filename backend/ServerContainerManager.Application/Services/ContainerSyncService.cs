using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Services
{
    internal class ContainerSyncService(ILogger<ContainerSyncService> logger, DockerClient dockerClient, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<ContainerSyncService> _logger = logger;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        private static readonly HashSet<string> TrackedActions = [DockerEventActions.Create, DockerEventActions.Destroy];
        private static readonly TimeSpan ErrorRetryDelay = TimeSpan.FromSeconds(5);
        private static readonly int MaxRetries = 10;

        private int _retriesCount = 0;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ReconcileAsync(stoppingToken);
            
            var containerEventsParameters = new ContainerEventsParameters();
            var progress = new Progress<Message>(async (message) => 
            {
                if (!TrackedActions.Contains(message.Action)) return;

                await HandleContainerEventAsync(message, stoppingToken);
            });

            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _dockerClient.System.MonitorEventsAsync(containerEventsParameters, progress, stoppingToken);
                    _retriesCount = 0;
                }
                catch(OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Cancellation requested, stopping service.");
                    break;
                }
                catch(Exception ex)
                {
                    if(_retriesCount == MaxRetries)
                    {
                        _logger.LogError(ex, "Max retries reached");
                        throw;
                    }

                    logger.LogError(ex, "Docker event stream disconnected. Reconnecting in 5s...");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    _retriesCount ++;
                }
            }
        }

        private async Task ReconcileAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reconciling all containers with Docker.");

            await using var scope = _serviceScopeFactory.CreateAsyncScope();
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

            _logger.LogInformation(
                "Reconciliation complete. {Added} added, {Removed} removed.",
                added,
                staleIds.Count);
        }

        private async Task HandleContainerEventAsync(Message message, CancellationToken stoppingToken)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            switch(message.Action)
            {
                case DockerEventActions.Create:
                    if (await db.Containers.FindAsync([message.ID], stoppingToken) != null) return;

                    var containerCreateResult = Container.Create(message.ID, []);
                    if (containerCreateResult.IsError)
                        throw new InvalidOperationException(string.Join('\n', containerCreateResult.Errors.Select(e => $"Code: {e.Code}, Description: {e.Description}")));

                    await db.Containers.AddAsync(containerCreateResult.Value, stoppingToken);
                    break;
                case DockerEventActions.Destroy:
                    var container = await db.Containers.FindAsync([message.ID], stoppingToken);
                    if(container == null) return;

                    db.Containers.Remove(container);
                    break;
                default:
                    throw new ArgumentException($"Invalid Action: {message.Action}.", nameof(message));
            }

            await db.SaveChangesAsync(stoppingToken);
        }
    }
}
