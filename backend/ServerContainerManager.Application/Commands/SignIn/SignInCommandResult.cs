using ErrorOr;

namespace ServerContainerManager.Application.Commands.SignIn
{
    public record SignInCommandResultNamespace(Guid Id, string Name);
    public record SignInCommandResult
    {
        public bool IsError { get; init; }
        public List<Error> Errors { get; init; }
        public Guid UserId { get; init; }
        public string Username { get; init; }
        public IEnumerable<string> Roles { get; init; }
        public IEnumerable<SignInCommandResultNamespace> Namespaces { get; init; }

        public SignInCommandResult(List<Error> errors)
        {
            IsError = true;
            Errors = [.. errors];
        }

        public SignInCommandResult(
            Guid userId,
            string username,
            IEnumerable<string> roles,
            IEnumerable<SignInCommandResultNamespace> namespaces)
        {
            IsError = false;
            UserId = userId;
            Username = username;
            Roles = [.. roles];
            Namespaces = [.. namespaces];
        }
    }
}
