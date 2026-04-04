

namespace ServerContainerManager.Application.Commands.User.GetUserList
{
    public sealed record GetUserListCommandResult
    {
        public required IList<Models.User> Users { get; init; }
    }
}
