namespace ServerContainerManager.API.Models.Responses.NamespacesController
{
    public sealed record GetNamespaceAssociatedContainersAssociatedContainersResponse
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
    }

    public sealed record GetNamespaceAssociatedContainersResponse
    {
        public required IReadOnlyCollection<GetNamespaceAssociatedContainersAssociatedContainersResponse> AssociatedContainers { get; init; }
    }
}
