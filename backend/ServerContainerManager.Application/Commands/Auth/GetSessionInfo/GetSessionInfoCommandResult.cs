namespace ServerContainerManager.Application.Commands.Auth.GetSessionInfo
{
    public sealed record GetSessionInfoCommandResult
    {
        public required Models.User User { get; init; }
    }
}
