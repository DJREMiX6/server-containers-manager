namespace ServerContainerManager.Application.Commands.Container.StartContainer
{
    public sealed record StartContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
