namespace ServerContainerManager.Application.Commands.Auth.GetSessionInfo
{
    public sealed record GetSessionInfoCommand
    {
        public required Guid UserId { get; init; }
    }
}
