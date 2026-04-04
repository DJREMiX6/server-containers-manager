using ServerContainerManager.API.Models.Responses.Common;

namespace ServerContainerManager.API.Models.Responses.UsersController
{
    public record GetUserListResponse
    {
        public required IList<UserResponseModel> Users { get; init; }
    }
}
