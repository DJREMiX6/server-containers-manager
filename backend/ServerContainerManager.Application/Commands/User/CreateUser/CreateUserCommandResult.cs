namespace ServerContainerManager.Application.Commands.User.CreateUser
{
    public sealed record CreateUserCommandResult
    {
        public required Guid UserId { get; init; }
    }
}
