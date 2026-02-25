using ServerContainerManager.Application.Commands.Models;

namespace ServerContainerManager.Application.Commands.GetUserList
{
    public sealed record GetUserListCommandResultUserInfo
    {
        public Guid Id { get; }
        public string Username { get; }
        public IList<string> Roles { get; }
        public IList<NamespaceInfo> Namespaces { get; }

        public GetUserListCommandResultUserInfo(
            Guid id,
            string username,
            IList<string> roles,
            IList<NamespaceInfo> namespaces)
        {
            Id = id;
            Username = username;
            Roles = [.. roles];
            Namespaces = [.. namespaces];
        }
    }

    public sealed record GetUserListCommandResult
    {
        public IList<GetUserListCommandResultUserInfo> Users { get; }

        public GetUserListCommandResult(IList<GetUserListCommandResultUserInfo> users)
        {
            Users = [.. users];
        }
    }
}
