namespace ServerContainerManager.Application.Commands.Namespace.UpdateNamespaceAssociatedContainers
{
    public sealed record UpdateNamespaceAssociatedContainersCommand
    {
        public required Guid NamespaceId { get; init; }
        public required IReadOnlyCollection<string> AssociatedContainerIds { get; init; }
    }
}
