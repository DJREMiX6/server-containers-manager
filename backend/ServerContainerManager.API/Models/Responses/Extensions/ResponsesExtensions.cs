using ServerContainerManager.API.Models.Responses.Models;
using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ResponsesExtensions
    {
        public static ContainerLabelResponseModel ToResponseModel(this ContainerLabel label) => ContainerLabelResponseModel.FromQueryModel(label);

        public static IReadOnlyCollection<ContainerLabelResponseModel> ToResponseModel(this IEnumerable<ContainerLabel> labels) => [.. labels.Select(ToResponseModel)];

        public static ContainerPortResponseModel ToResponseModel(this ContainerPort port) => ContainerPortResponseModel.FromQueryModel(port);

        public static IReadOnlyCollection<ContainerPortResponseModel> ToResponseModel(this IEnumerable<ContainerPort> ports) => [.. ports.Select(ToResponseModel)];

        public static NamespaceInfoResponseModel ToResponseModel(this NamespaceInfo namespaceInfo) => NamespaceInfoResponseModel.FromQueryModel(namespaceInfo);

        public static IReadOnlyCollection<NamespaceInfoResponseModel> ToResponseModel(this IEnumerable<NamespaceInfo> namespacesInfo) => [.. namespacesInfo.Select(ToResponseModel)];
    } 
}
