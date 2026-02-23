using ServerContainerManager.Application.Commands.GetUserList;

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

    }
}
