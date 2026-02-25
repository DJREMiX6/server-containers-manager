using ServerContainerManager.API.Models.Responses.Models;

namespace ServerContainerManager.API.Models.Responses.AuthController
{
    public sealed record GetSessionInfoResponse
    {
        public Guid Id { get; }
        public string Username { get; }
        public IList<string> Roles { get; }
        public IList<NamespaceInfoResponseModel> Namespaces { get; }

        public GetSessionInfoResponse(Guid id, string username, IList<string> roles, IList<NamespaceInfoResponseModel> namespaces)
        {
            Id = id;
            Username = username;
            Roles = [.. roles];
            Namespaces = [.. namespaces];
        }
    }
}
