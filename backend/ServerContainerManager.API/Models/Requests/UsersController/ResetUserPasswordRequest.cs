namespace ServerContainerManager.API.Models.Requests.UsersController
{
    public sealed record ResetUserPasswordRequest
    {
        public required Guid UserId { get; init; }
        public required string Password { get; init; }
    }
}
