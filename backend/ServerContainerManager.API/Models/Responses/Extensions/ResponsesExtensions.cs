using ServerContainerManager.API.Models.Responses.Models;
using ServerContainerManager.Application.Commands.Models;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ResponsesExtensions
    {
        public static NamespaceInfoResponseModel ToResponseModel(this NamespaceInfo namespaceInfo) => NamespaceInfoResponseModel.FromQueryModel(namespaceInfo);

        public static IList<NamespaceInfoResponseModel> ToResponseModel(this IEnumerable<NamespaceInfo> namespacesInfo) => [.. namespacesInfo.Select(ToResponseModel)];
    } 
}
