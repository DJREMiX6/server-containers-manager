namespace ServerContainerManager.Application.Commands.ResumeContainer
{
    public sealed record ResumeContainerCommand
    {
        public required Guid UserId { get; init; }
        public required string ContainerId { get; init; }
    }
}
