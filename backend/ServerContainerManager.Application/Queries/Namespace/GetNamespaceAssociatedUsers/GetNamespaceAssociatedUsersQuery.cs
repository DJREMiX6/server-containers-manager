namespace ServerContainerManager.Application.Queries.Namespace.GetNamespaceAssociatedUsers
{
    public sealed record GetNamespaceAssociatedUsersQuery
    {
        public required Guid NamespaceId { get; init; }
    }
}
