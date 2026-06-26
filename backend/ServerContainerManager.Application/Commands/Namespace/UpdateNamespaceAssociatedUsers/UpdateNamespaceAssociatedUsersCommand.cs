namespace ServerContainerManager.Application.Commands.Namespace.UpdateNamespaceAssociatedUsers
{
    public sealed record UpdateNamespaceAssociatedUsersCommand
    {
        public required Guid NamespaceId { get; init; }
        public required IReadOnlyCollection<Guid> AssociatedUserIds { get; init; }
    }
}
