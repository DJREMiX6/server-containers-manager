namespace ServerContainerManager.API.Models.Requests.NamespacesController
{
    public sealed record CreateNamespaceRequest
    {
        public required string Name { get; init; }
    }
}
