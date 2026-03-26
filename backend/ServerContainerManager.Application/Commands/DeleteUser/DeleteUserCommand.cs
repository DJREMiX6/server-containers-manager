namespace ServerContainerManager.Application.Commands.DeleteUser
{
    public sealed record DeleteUserCommand
    {
        public required Guid UserId { get; init; }
    }
}
