namespace ServerContainerManager.Application.Commands.StartContainer
{
    public record StartContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
