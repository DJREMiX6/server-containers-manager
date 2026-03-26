namespace ServerContainerManager.Application.Commands.User.UpdateUserNamespaces
{
    public sealed record UpdateUserNamespacesCommand
    {
        public required Guid UserId { get; init; }
        public required IList<Guid> NamespacesIds { get; init; }
    }
}
