namespace ServerContainerManager.Application.Commands.Container.UpdateContainerNamespaces
{
    public sealed record UpdateContainerNamespacesCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
        public required IReadOnlyCollection<Guid> NamespacesIds { get; init; }
    }
}
