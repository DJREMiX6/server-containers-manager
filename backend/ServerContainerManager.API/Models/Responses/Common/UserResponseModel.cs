using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Responses.Common
{
    public sealed record UserResponseModel
    {
        public required Guid UserId { get; init; }
        public required string Username { get; init; }
        public required IList<string> Roles { get; init; }
        public required IList<NamespaceInfoResponseModel> Namespaces { get; init; }
        public required bool IsConfirmed { get; init; }

        public static UserResponseModel FromQueryModel(User user) => new()
        { 
            UserId = user.UserId,
            Username = user.Username,
            Roles = [.. user.Roles],
            Namespaces = user.Namespaces.Select(NamespaceInfoResponseModel.FromQueryModel).ToList(),
            IsConfirmed = user.IsConfirmed,
        };
    }
}
