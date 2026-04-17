using ServerContainerManager.API.Models.Responses.NamespacesController;
using ServerContainerManager.Application.Commands.Namespace.CreateNamespace;
using ServerContainerManager.Application.Queries.Namespace.GetNamespacesList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class NamespacesControllerResponsesExtensions
    {
        public static GetNamespacesListResponse ToContract(this GetNamespacesListQueryResult result) => new()
        {
            Namespaces = result.Namespaces.ToResponseModel(),
            TotalCount = result.TotalCount
        };

        public static CreateNamespaceResponse ToContract(this CreateNamespaceCommandResult result) => new()
        {
            NamespaceId = result.NamespaceId
        };
    }
}
