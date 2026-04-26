namespace ServerContainerManager.Application.Queries.User.CheckUsernameAvailability
{
    public sealed record CheckUsernameAvailabilityQuery
    {
        public required string Username { get; init; }
    }
}
