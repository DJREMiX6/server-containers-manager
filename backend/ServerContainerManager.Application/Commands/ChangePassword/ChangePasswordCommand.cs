namespace ServerContainerManager.Application.Commands.ChangePassword
{
    public sealed record ChangePasswordCommand
    {
        public required Guid UserId { get; init; }
        public required string CurrentPassword { get; init; }
        public required string NewPassword { get; init; }
    }
}
