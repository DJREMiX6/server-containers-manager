using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServerContainerManager.Application.Options;

namespace ServerContainerManager.Application.Services
{
    internal class PeriodicContainersReconciliator(
        ILogger<PeriodicContainersReconciliator> logger, 
        DockerContainersEventsSignalsQueue signalsQueue,
        IOptions<DockerContainersReconciliationOptions> reconciliationOptions) : BackgroundService
    {
        private readonly ILogger<PeriodicContainersReconciliator> _logger = logger;
        private readonly DockerContainersEventsSignalsQueue signalsQueue = signalsQueue;
        private readonly IOptions<DockerContainersReconciliationOptions> _reconciliationOptions = reconciliationOptions;
        
        private TimeSpan PeriodicDelay => TimeSpan.FromMilliseconds(_reconciliationOptions.Value.PeriodicReconciliationDelayMs);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var periodicTimer = new PeriodicTimer(PeriodicDelay);
            _logger.LogInformation("Set Periodic Timer delay to {PeriodicDelayMilliseconds} milliseconds", _reconciliationOptions.Value.PeriodicReconciliationDelayMs);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Signaled the ContainersEventsSignalsQueue to Reconciliate");
                await signalsQueue.WriteAsync(true, stoppingToken); 
                _logger.LogInformation("Waiting for Periodic Timer tick in {PeriodicDelayMilliseconds} milliseconds", _reconciliationOptions.Value.PeriodicReconciliationDelayMs);
                await periodicTimer.WaitForNextTickAsync(stoppingToken);
            }
        }
    }
}
