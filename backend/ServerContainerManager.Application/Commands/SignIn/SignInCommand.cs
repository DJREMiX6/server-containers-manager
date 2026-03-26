namespace ServerContainerManager.Application.Commands.SignIn
{
    public sealed record SignInCommand
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required bool IsPersistent { get; init; }
        public required bool LockOutOnFailure { get; init; }
    }
}
