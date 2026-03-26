namespace ServerContainerManager.Application.Commands.DeleteUser
{
    public sealed record DeleteUserCommand
    {
        public Guid UserId { get; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
