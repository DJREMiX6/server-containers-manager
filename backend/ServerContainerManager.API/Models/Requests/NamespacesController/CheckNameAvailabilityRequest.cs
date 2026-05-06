namespace ServerContainerManager.API.Models.Requests.NamespacesController
{
    public sealed record CheckNameAvailabilityRequest
    {
        public required string Name { get; init; }
    }
}
