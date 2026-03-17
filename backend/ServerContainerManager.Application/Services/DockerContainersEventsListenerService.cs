using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServerContainerManager.Application.Options;

namespace ServerContainerManager.Application.Services
{
    internal class DockerContainersEventsListenerService(
        ILogger<DockerContainersEventsListenerService> logger,
        DockerClient dockerClient,
        DockerContainersEventsSignalsQueue signalsQueue,
        IOptions<DockerContainersReconciliationOptions> reconciliationOptions) : BackgroundService
    {
        private readonly ILogger<DockerContainersEventsListenerService> _logger = logger;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly DockerContainersEventsSignalsQueue _signalsQueue = signalsQueue;
        private readonly IOptions<DockerContainersReconciliationOptions> _reconciliationOptions = reconciliationOptions;

        private readonly ContainerEventsParameters containerEventsParameters = new();

        private TimeSpan ErrorRetryDelay => TimeSpan.FromMilliseconds(_reconciliationOptions.Value.DockerConnectionRetryDelayMs);
        private uint MaxRetries => _reconciliationOptions.Value.DockerConnectionMaxRetries;
        private uint RetriesCount { get; set; } = 0;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var progress = new Progress<Message>((message) =>
            {
                var writeResult = _signalsQueue.TryWrite(true);

                if (!writeResult)
                    _logger.LogError("Unable to write the following message: {Message}", message);
            });

            while (stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    await _dockerClient.System.MonitorEventsAsync(containerEventsParameters, progress, stoppingToken);
                    RetriesCount = 0;
                }
                catch(OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Cancellation requested, stopping service.");
                    break;
                }
                catch(Exception ex)
                {
                    if (RetriesCount >= MaxRetries)
                    {
                        _logger.LogError(ex, "Max retries reached");
                        throw;
                    }

                    _logger.LogError(ex, "Docker event stream disconnected. Reconnecting in 5s...");
                    await Task.Delay(ErrorRetryDelay, stoppingToken);
                    RetriesCount++;
                }
            }
        }
    }
}
