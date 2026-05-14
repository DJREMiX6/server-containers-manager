namespace ServerContainerManager.Application.Queries.Auth.GetSessionInfo
{
    public sealed record GetSessionInfoQueryResult
    {
        public required Models.User User { get; init; }
    }
}
