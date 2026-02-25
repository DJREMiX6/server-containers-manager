namespace ServerContainerManager.Application.Commands.CreateUser
{
    public record CreateUserCommandResult
    {
        public Guid UserId { get; }

        public CreateUserCommandResult(Guid userId)
        {
            UserId = userId;
        }
    }
}
