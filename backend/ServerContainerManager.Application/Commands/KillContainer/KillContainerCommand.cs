namespace ServerContainerManager.Application.Commands.KillContainer
{
    public sealed record KillContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
