using ServerContainerManager.Application.Commands.GetUserList;
using ServerContainerManager.Application.Commands.SignIn;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class AuthControllerResponsesExtensions
    {
        public static IEnumerable<GetUserListResponse> ToContract(this IEnumerable<GetUserListCommandResult> result) => result
            .Select(r => new GetUserListResponse(
                Id: r.Id,
                Username: r.Username,
                Roles: r.Roles,
                Namespaces: r.Namespaces
                    .Select(n => new GetUserListResponseNamespace(Id: n.Id, Name: n.Name))));

        public static SignInResponse ToContract(this SignInCommandResult result) => new(
            Id: result.UserId,
            Username: result.Username,
            Roles: result.Roles,
            Namespaces: result.Namespaces
                .Select(n => new SignInResponseNamespace(Id: n.Id, Name: n.Name)));
    }
}
