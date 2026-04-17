namespace ServerContainerManager.Application.Queries.Namespace.GetNamespacesList
{
    public sealed record GetNamespacesListQuery
    {
        public required Guid UserId { get; init; }
    }
}
