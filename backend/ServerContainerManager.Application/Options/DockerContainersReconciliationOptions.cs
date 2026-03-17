namespace ServerContainerManager.Application.Options
{
    public sealed class DockerContainersReconciliationOptions
    {
        public const string SectionName = "DockerContainersReconciliation";

        public uint PeriodicReconciliationDelayMs { get; set; } = 10_000;
        public uint EventsSignalsProcessingDelayMs { get; set; } = 1_000;
        public uint DockerConnectionMaxRetries { get; set; } = 0;
        public uint DockerConnectionRetryDelayMs { get; set; } = 100;
    }
}
