namespace ServerContainerManager.Application.Queries.Namespace.GetNamespacesList
{
    public sealed record GetNamespacesListQueryResultNamespace
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required int AssociatedUsersCount { get; init; }
        public required int AssociatedContainersCount { get; init; }
    }
    public sealed record GetNamespacesListQueryResult
    {
        public required IReadOnlyCollection<GetNamespacesListQueryResultNamespace> Namespaces { get; init; }
        public required int TotalCount { get; init; }
    }
}
