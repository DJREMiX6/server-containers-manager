using ServerContainerManager.API.Models.Responses.Common;
using ServerContainerManager.API.Models.Responses.UsersController;
using ServerContainerManager.Application.Commands.User.GetUserList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class UsersControllerResponseExtensions
    {
        public static GetUserListResponse ToContract(this GetUserListCommandResult result) => new()
        {
            Users = [.. result.Users.Select(UserResponseModel.FromQueryModel)]
        };
    }
}
