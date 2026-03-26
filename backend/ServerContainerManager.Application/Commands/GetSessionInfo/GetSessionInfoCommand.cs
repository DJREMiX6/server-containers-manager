namespace ServerContainerManager.Application.Commands.GetSessionInfo
{
    public sealed record GetSessionInfoCommand
    {
        public Guid UserId { get; private set; }

        public GetSessionInfoCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
