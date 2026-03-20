using ServerContainerManager.API.Models.Responses.NamespacesController;
using ServerContainerManager.Application.Queries.GetNamespacesList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class NamespacesControllerResponsesExtensions
    {
        public static GetNamespacesListResponse ToContract(this GetNamespacesListQueryResult result) => new()
        {
            Namespaces = result.Namespaces.ToResponseModel(),
            TotalCount = result.TotalCount
        };
    }
}
