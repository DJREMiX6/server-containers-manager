namespace ServerContainerManager.Application.Queries.Namespace.CheckNamespaceNameAvailability
{
    public sealed record CheckNamespaceNameAvailabilityQueryResult
    {
        public required bool IsAvailable { get; init; }
    }
}
