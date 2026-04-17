using ServerContainerManager.API.Models.Responses.Common;
using ServerContainerManager.API.Models.Responses.UsersController;
using ServerContainerManager.Application.Queries.User.GetUserList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class UsersControllerResponseExtensions
    {
        public static GetUserListUserResponse ToContract(this GetUserListQueryUserResult resultl) => new()
        {
            Id = resultl.Id,
            Username = resultl.Username,
            Namespaces = [.. resultl.Namespaces.ToResponseModel()],
            Roles = resultl.Roles,
            IsConfirmed = resultl.IsConfirmed,
            LastLoginDate = resultl.LastLoginDate,
        };

        public static GetUserListResponse ToContract(this GetUserListQueryResult result) => new()
        {
            Users = [.. result.Users.Select(ToContract)]
        };
    }
}
