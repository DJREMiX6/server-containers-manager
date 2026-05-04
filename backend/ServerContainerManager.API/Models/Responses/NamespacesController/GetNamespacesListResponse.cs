using ServerContainerManager.API.Models.Responses.Common;

namespace ServerContainerManager.API.Models.Responses.NamespacesController
{
    public sealed record GetNamespacesListResponse
    {
        public required IReadOnlyCollection<GetNamespacesListResponseNamespace> Namespaces { get; init; }
        public required int TotalCount { get; init; }
    }
}
