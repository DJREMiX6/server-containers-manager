namespace ServerContainerManager.Application.Commands.GetSessionInfo
{
    public sealed record GetSessionInfoCommand
    {
        public required Guid UserId { get; init; }
    }
}
