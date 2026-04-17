namespace ServerContainerManager.API.Models.Requests.UsersController
{
    public sealed record ResetUserPasswordRequest
    {
        public required string Password { get; init; }
    }
}
