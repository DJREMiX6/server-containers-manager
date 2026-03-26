namespace ServerContainerManager.Application.Commands.Container.RestartContainer
{
    public sealed record RestartContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
