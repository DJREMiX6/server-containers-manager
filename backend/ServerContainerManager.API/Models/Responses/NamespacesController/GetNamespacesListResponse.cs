using ServerContainerManager.API.Models.Responses.Models;

namespace ServerContainerManager.API.Models.Responses.NamespacesController
{
    public sealed record GetNamespacesListResponse
    {
        public required IReadOnlyCollection<NamespaceInfoResponseModel> Namespaces { get; init; }
        public required int TotalCount { get; init; }
    }
}
