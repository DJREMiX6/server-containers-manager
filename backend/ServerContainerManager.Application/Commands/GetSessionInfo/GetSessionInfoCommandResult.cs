using ServerContainerManager.Application.Commands.SignIn;
using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Commands.GetSessionInfo
{
    public record GetSessionInfoCommandResult
    {
        public Guid UserId { get; init; }
        public string Username { get; init; }
        public IList<string> Roles { get; init; }
        public IList<NamespaceInfo> Namespaces { get; init; }

        public GetSessionInfoCommandResult(
            Guid userId,
            string username,
            IList<string> roles,
            IList<NamespaceInfo> namespaces)
        {
            UserId = userId;
            Username = username;
            Roles = [.. roles];
            Namespaces = [.. namespaces];
        }
    }
}
