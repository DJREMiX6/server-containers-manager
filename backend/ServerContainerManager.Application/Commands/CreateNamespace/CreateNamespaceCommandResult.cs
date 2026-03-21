namespace ServerContainerManager.Application.Commands.CreateNamespace
{
    public sealed record CreateNamespaceCommandResult
    {
        public required Guid NamespaceId { get; init; }
    }
}
