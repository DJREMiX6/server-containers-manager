namespace ServerContainerManager.Application.Queries.GetNamespacesList
{
    public sealed record GetNamespacesListQuery
    {
        public required Guid UserId { get; init; }
    }
}
