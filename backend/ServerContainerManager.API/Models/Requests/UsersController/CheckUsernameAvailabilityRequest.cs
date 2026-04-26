namespace ServerContainerManager.API.Models.Requests.UsersController
{
    public sealed record CheckUsernameAvailabilityRequest
    {
        public required string Username { get; init; }
    }
}
