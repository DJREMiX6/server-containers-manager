namespace ServerContainerManager.API.Models.Requests.ContainersController
{
    public sealed record UpdateContainerNamespacesRequest
    {
        public required IReadOnlyCollection<Guid> NamespacesIds { get; init; }
    }
}
