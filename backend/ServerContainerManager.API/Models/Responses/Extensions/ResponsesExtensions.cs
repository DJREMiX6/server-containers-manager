using ServerContainerManager.API.Models.Responses.Models;
using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ResponsesExtensions
    {
        public static NamespaceInfoResponseModel ToResponseModel(this NamespaceInfo namespaceInfo) => NamespaceInfoResponseModel.FromQueryModel(namespaceInfo);

        public static IReadOnlyCollection<NamespaceInfoResponseModel> ToResponseModel(this IEnumerable<NamespaceInfo> namespacesInfo) => [.. namespacesInfo.Select(ToResponseModel)];
    } 
}
