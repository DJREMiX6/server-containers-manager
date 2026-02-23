using ServerContainerManager.Application.Commands.SignIn;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class AuthControllerResponsesExtensions
    {
        public static SignInResponse ToContract(this SignInCommandResult result) => new(
            Id: result.UserId,
            Username: result.Username,
            Roles: result.Roles,
            Namespaces: result.Namespaces
                .Select(n => new SignInResponseNamespace(Id: n.Id, Name: n.Name)));
    }
}
