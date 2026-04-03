namespace ServerContainerManager.Application.Commands.Namespace.CreateNamespace
{
    public sealed record CreateNamespaceCommand
    {
        public required string Name { get; init; }
    }
}
