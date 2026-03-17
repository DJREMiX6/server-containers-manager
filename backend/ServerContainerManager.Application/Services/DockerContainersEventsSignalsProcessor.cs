using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServerContainerManager.Application.Options;

namespace ServerContainerManager.Application.Services
{
    internal class DockerContainersEventsSignalsProcessor(
        ILogger<DockerContainersEventsSignalsProcessor> logger,
        DockerContainersEventsSignalsQueue signalsQueue,
        DockerContainersReconciliator reconciliator,
        IOptions<DockerContainersReconciliationOptions> reconciliationOptions) : BackgroundService
    {
        private readonly ILogger<DockerContainersEventsSignalsProcessor> _logger = logger;
        private readonly DockerContainersEventsSignalsQueue _signalsQueue = signalsQueue;
        private readonly DockerContainersReconciliator _reconciliator = reconciliator;
        private readonly IOptions<DockerContainersReconciliationOptions> _reconciliationOptions = reconciliationOptions;
        
        private TimeSpan DebounceDelay => TimeSpan.FromMilliseconds(_reconciliationOptions.Value.EventsSignalsProcessingDelayMs);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await _signalsQueue.ReadAsync(stoppingToken);

                var debounceTimer = Task.Delay(DebounceDelay, stoppingToken);

                while (true)
                {
                    var nextSignal = _signalsQueue.WaitToReadAsync(stoppingToken).AsTask();
                    var completed = await Task.WhenAny(debounceTimer, nextSignal);

                    if (completed == debounceTimer)
                        break;

                    while (_signalsQueue.TryRead(out _)) { } // Consumes all signals
                }

                try
                {
                    await _reconciliator.ReconciliateAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Reconciliation failed.");
                }
            }
        }
    }
}
