namespace ServerContainerManager.Application.Commands.ChangeUsername
{
    public record ChangeUsernameCommand
    {
        public Guid UserId { get; }
        public string NewUsername { get; }

        public ChangeUsernameCommand(Guid userId, string newUsername)
        {
            UserId = userId;
            NewUsername = newUsername;
        }
    }
}
