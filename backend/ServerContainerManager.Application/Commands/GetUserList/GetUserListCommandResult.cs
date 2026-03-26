using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Commands.GetUserList
{
    public sealed record GetUserListCommandResultUserInfo
    {
        public required Guid Id { get; init; }
        public required string Username { get; init; }
        public required IList<string> Roles { get; init; }
        public required IList<NamespaceInfo> Namespaces { get; init; }
    }

    public sealed record GetUserListCommandResult
    {
        public required IList<GetUserListCommandResultUserInfo> Users { get; init; }
    }
}
