namespace ServerContainerManager.Application.Queries.Auth.GetSessionInfo
{
    public sealed record GetSessionInfoQuery
    {
        public required Guid UserId { get; init; }
    }
}
