namespace ServerContainerManager.Application.Queries.Namespace.CheckNamespaceNameAvailability
{
    public sealed record CheckNamespaceNameAvailabilityQuery
    {
        public required string Name { get; init; }
    }
}
