using ServerContainerManager.API.Models.Responses.AuthController;
using ServerContainerManager.Application.Commands.GetSessionInfo;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class AuthControllerResponsesExtensions
    {
        public static GetSessionInfoResponse ToContract(this GetSessionInfoCommandResult result) => new(
            id: result.UserId,
            username: result.Username,
            roles: result.Roles,
            namespaces: [.. result.Namespaces.ToResponseModel()]);
    }
}
