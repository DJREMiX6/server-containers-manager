using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Queries.User.GetUserList
{
    public sealed record GetUserListQueryUserResult
    {
        public required Guid Id { get; init; }
        public required string Username { get; init; }
        public required IList<string> Roles { get; init; }
        public required IList<NamespaceInfo> Namespaces { get; init; }
        public required bool IsConfirmed { get; init; }
        public required DateTime? LastLoginDate { get; init; }
    }

    public sealed record GetUserListQueryResult
    {
        public required IList<GetUserListQueryUserResult> Users { get; init; }
    }
}
