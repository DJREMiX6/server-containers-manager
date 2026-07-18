namespace ServerContainerManager.Application.Queries.Namespace.GetNamespaceAssociatedContainers
{
    public record GetNamespaceAssociatedContainersQuery
    {
        public required Guid NamespaceId { get; init; }
    }
}
