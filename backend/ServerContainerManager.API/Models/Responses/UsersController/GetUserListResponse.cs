using ServerContainerManager.API.Models.Responses.Common;

namespace ServerContainerManager.API.Models.Responses.UsersController
{
    public record GetUserListUserResponse
    {
        public required Guid Id { get; init; }
        public required string Username { get; init; }
        public required IList<string> Roles { get; init; }
        public required IList<NamespaceInfoResponseModel> Namespaces { get; init; }
        public required bool IsConfirmed { get; init; }
        public required DateTime? LastLoginDate { get; init; }
    }

    public record GetUserListResponse
    {
        public required IList<GetUserListUserResponse> Users { get; init; }
    }
}
