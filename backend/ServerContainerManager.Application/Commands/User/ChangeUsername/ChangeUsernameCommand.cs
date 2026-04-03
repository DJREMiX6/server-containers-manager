namespace ServerContainerManager.Application.Commands.User.ChangeUsername
{
    public sealed record ChangeUsernameCommand
    {
        public required Guid UserId { get; init; }
        public required string NewUsername { get; init; }
    }
}
