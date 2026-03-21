namespace ServerContainerManager.Application.Commands.CreateNamespace
{
    public sealed record CreateNamespaceCommand
    {
        public required string Name { get; init; }
    }
}
