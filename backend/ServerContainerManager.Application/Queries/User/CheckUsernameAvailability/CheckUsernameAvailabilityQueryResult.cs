namespace ServerContainerManager.Application.Queries.User.CheckUsernameAvailability
{
    public sealed record CheckUsernameAvailabilityQueryResult
    {
        public required bool IsAvailable { get; init; }
    }
}
