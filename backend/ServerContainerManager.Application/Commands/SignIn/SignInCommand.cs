namespace ServerContainerManager.Application.Commands.SignIn
{
    public sealed record SignInCommand
    {
        public string Username { get; }
        public string Password { get; }
        public bool IsPersistent { get; }
        public bool LockOutOnFailure { get; }

        public SignInCommand(string username, string password, bool isPersistent, bool lockOutOnFailure)
        {
            Username = username;
            Password = password;
            IsPersistent = isPersistent;
            LockOutOnFailure = lockOutOnFailure;
        }
    }
}
