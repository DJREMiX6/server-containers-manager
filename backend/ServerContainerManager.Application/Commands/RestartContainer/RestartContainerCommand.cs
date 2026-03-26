namespace ServerContainerManager.Application.Commands.RestartContainer
{
    public record RestartContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
