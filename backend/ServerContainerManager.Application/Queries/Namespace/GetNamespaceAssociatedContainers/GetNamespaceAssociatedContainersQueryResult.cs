namespace ServerContainerManager.Application.Queries.Namespace.GetNamespaceAssociatedContainers
{
    public sealed record AssociatedContainer
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
    }

    public sealed record GetNamespaceAssociatedContainersQueryResult
    {
        public required IReadOnlyCollection<AssociatedContainer> AssociatedContainers { get; init; }
    }
}
