using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Queries.Namespace.GetNamespacesList
{
    public sealed record GetNamespacesListQueryResult
    {
        public required IReadOnlyCollection<NamespaceInfo> Namespaces { get; init; }
        public required int TotalCount { get; init; }
    }
}
