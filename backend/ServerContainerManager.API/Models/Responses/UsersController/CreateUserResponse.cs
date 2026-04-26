namespace ServerContainerManager.API.Models.Responses.UsersController
{
    public sealed record CreateUserResponse
    {
        public required Guid UserId { get; init; }
    }
}
