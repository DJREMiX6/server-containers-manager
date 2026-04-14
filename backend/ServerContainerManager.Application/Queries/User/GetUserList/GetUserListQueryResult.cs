namespace ServerContainerManager.Application.Queries.User.GetUserList
{
    public sealed record GetUserListQueryResult
    {
        public required IList<Models.User> Users { get; init; }
    }
}
