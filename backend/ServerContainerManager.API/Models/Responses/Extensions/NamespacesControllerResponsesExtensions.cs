using ServerContainerManager.API.Models.Responses.NamespacesController;
using ServerContainerManager.Application.Commands.Namespace.CreateNamespace;
using ServerContainerManager.Application.Queries.Namespace.GetNamespacesList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class NamespacesControllerResponsesExtensions
    {
        public static GetNamespacesListResponse ToContract(this GetNamespacesListQueryResult result) => new()
        {
            Namespaces = [.. result.Namespaces.ToResponseModel()],
            TotalCount = result.TotalCount
        };

        public static CreateNamespaceResponse ToContract(this CreateNamespaceCommandResult result) => new()
        {
            NamespaceId = result.NamespaceId
        };

        private static GetNamespacesListResponseNamespace ToResponseModel(this GetNamespacesListQueryResultNamespace result) => new()
        {
            Id = result.Id,
            Name = result.Name,
            AssociatedContainersCount = result.AssociatedContainersCount,
            AssociatedUsersCount = result.AssociatedUsersCount,
        };

        private static IEnumerable<GetNamespacesListResponseNamespace> ToResponseModel(this IEnumerable<GetNamespacesListQueryResultNamespace> results) => results.Select(ToResponseModel);
    }
}
