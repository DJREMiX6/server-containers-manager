namespace ServerContainerManager.API.Models.Requests.NamespacesController
{
    public sealed record UpdateNamespaceUsersRequest
    {
        public required IReadOnlyCollection<Guid> AssociatedUserIds { get; init; }
    }
}
