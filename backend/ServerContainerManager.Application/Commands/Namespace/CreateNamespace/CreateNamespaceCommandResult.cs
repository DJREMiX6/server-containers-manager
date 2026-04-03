namespace ServerContainerManager.Application.Commands.Namespace.CreateNamespace
{
    public sealed record CreateNamespaceCommandResult
    {
        public required Guid NamespaceId { get; init; }
    }
}
