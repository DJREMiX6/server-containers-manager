using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Commands.GetSessionInfo
{
    public sealed record GetSessionInfoCommandResult
    {
        public required Guid UserId { get; init; }
        public required string Username { get; init; }
        public required IList<string> Roles { get; init; }
        public required IList<NamespaceInfo> Namespaces { get; init; }
    }
}
