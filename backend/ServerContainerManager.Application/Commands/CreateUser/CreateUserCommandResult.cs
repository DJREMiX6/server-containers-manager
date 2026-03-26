namespace ServerContainerManager.Application.Commands.CreateUser
{
    public sealed record CreateUserCommandResult
    {
        public required Guid UserId { get; init; }
    }
}
