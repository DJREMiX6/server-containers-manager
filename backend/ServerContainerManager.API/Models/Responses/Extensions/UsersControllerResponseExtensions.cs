using ServerContainerManager.API.Models.Responses.UsersController;
using ServerContainerManager.Application.Commands.User.GetUserList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class UsersControllerResponseExtensions
    {
        public static GetUserListResponse ToContract(this GetUserListCommandResult result) => new(
            result.Users
            .Select(r => new GetUserListItemResponse(
                id: r.Id,
                username: r.Username,
                roles: [.. r.Roles],
                namespaces: [.. r.Namespaces.ToResponseModel()]))
            .ToList());
    }
}
