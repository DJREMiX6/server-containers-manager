namespace ServerContainerManager.Application.Commands.UpdateUserNamespaces
{
    public sealed record UpdateUserNamespacesCommand
    {
        public Guid UserId { get; }
        public IList<Guid> NamespacesIds { get; }

        public UpdateUserNamespacesCommand(Guid userId, IList<Guid> namespacesIds)
        {
            UserId = userId;
            NamespacesIds = namespacesIds;
        }
    }
}
