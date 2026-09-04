namespace ServerContainerManager.API.Models.Requests.NamespacesController
{
    public sealed record UpdateNamespaceContainersRequest
    {
        public required IReadOnlyCollection<string> AssociatedContainersIds { get; init; }
    }
}
