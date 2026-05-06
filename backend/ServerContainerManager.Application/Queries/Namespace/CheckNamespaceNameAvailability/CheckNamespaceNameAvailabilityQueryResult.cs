namespace ServerContainerManager.Application.Queries.Namespace.CheckNamespaceNameAvailability
{
    public sealed record CheckNamespaceNameAvailabilityQueryResult
    {
        public bool IsAvailable { get; init; }
    }
}
