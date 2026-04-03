namespace ServerContainerManager.Application.Commands.User.DeleteUser
{
    public sealed record DeleteUserCommand
    {
        public required Guid UserId { get; init; }
    }
}
