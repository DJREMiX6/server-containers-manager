namespace ServerContainerManager.API.Models.Responses.NamespacesController
{
    public sealed record GetNamespaceUsersAssociatedUserResponse
    {
        public required Guid Id { get; init; }
        public required string Username { get; init; }
    }

    public sealed record GetNamespaceUsersResponse
    {
        public required IReadOnlyCollection<GetNamespaceUsersAssociatedUserResponse> AssociatedUsers { get; init; }
    }
}
