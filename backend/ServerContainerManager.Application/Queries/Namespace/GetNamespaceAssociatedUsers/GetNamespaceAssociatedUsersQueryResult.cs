namespace ServerContainerManager.Application.Queries.Namespace.GetNamespaceAssociatedUsers
{
    public sealed record NamespaceAssociatedUser
    {
        public required Guid Id { get; init; }
        public required string Username { get; init; }
    }

    public sealed record GetNamespaceAssociatedUsersQueryResult
    {
        public required IReadOnlyCollection<NamespaceAssociatedUser> AssociatedUsers { get; init; }
    }
}
