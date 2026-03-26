namespace ServerContainerManager.Application.Commands.CreateUser
{
    public sealed record CreateUserCommand
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
