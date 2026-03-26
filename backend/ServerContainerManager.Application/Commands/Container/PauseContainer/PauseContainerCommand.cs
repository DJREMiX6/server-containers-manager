namespace ServerContainerManager.Application.Commands.Container.PauseContainer
{
    public sealed record PauseContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
