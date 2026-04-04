using ServerContainerManager.API.Models.Responses.AuthController;
using ServerContainerManager.Application.Commands.Auth.GetSessionInfo;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class AuthControllerResponsesExtensions
    {
        public static GetSessionInfoResponse ToContract(this GetSessionInfoCommandResult result) => new()
        {
            User = new()
            {
                UserId = result.User.UserId,
                Username = result.User.Username,
                Roles = result.User.Roles,
                Namespaces = [.. result.User.Namespaces.ToResponseModel()],
                IsConfirmed = result.User.IsConfirmed
            }
        };
    }
}
