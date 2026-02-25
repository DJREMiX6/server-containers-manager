using ServerContainerManager.API.Models.Responses.Models;

namespace ServerContainerManager.API.Models.Responses.UsersController
{
    public record GetUserListItemResponse
    {
        public Guid Id { get; }
        public string Username { get; }
        public IList<string> Roles { get; }
        public IList<NamespaceInfoResponseModel> Namespaces { get; }

        public GetUserListItemResponse(
            Guid id,
            string username,
            IList<string> roles,
            IList<NamespaceInfoResponseModel> namespaces)
        {
            Id = id;
            Username = username;
            Roles = roles;
            Namespaces = namespaces;
        }
    }
    public record GetUserListResponse
    {
        public IList<GetUserListItemResponse> Items { get; }

        public GetUserListResponse(IList<GetUserListItemResponse> items)
        {
            Items = items;
        }
    }
}
