namespace ServerContainerManager.Application.Commands.User.ResetPassword
{
    public sealed record ResetUserPasswordCommand
    {
        public required Guid UserId { get; init; }
        public required string Password { get; init; }
    }
}
