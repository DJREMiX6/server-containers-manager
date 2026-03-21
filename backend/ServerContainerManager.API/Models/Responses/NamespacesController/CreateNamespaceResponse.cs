namespace ServerContainerManager.API.Models.Responses.NamespacesController
{
    public sealed record CreateNamespaceResponse
    {
        public required Guid NamespaceId { get; init; }
    }
}
