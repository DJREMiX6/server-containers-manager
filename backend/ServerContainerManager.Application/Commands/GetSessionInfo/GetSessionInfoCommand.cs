using System.Security.Claims;

namespace ServerContainerManager.Application.Commands.GetSessionInfo
{
    public record GetSessionInfoCommand
    {
        public Guid UserId { get; private set; }

        public GetSessionInfoCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
