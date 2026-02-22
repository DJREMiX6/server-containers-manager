namespace ServerContainerManager.Application.Commands.SignIn
{
    public sealed record SignInCommand(string Username, string Password, bool IsPersistent, bool LockOutOnFailure);
}
