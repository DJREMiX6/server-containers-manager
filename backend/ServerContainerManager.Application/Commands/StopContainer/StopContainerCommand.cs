namespace ServerContainerManager.Application.Commands.StopContainer
{
    public sealed record StopContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
