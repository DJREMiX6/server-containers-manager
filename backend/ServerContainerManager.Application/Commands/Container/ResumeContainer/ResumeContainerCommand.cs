namespace ServerContainerManager.Application.Commands.Container.ResumeContainer
{
    public sealed record ResumeContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
