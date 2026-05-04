namespace ServerContainerManager.API.Models.Responses.NamespacesController
{
    public sealed record GetNamespacesListResponseNamespace
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required int AssociatedUsersCount { get; init; }
        public required int AssociatedContainersCount { get; init; }
    }
}
