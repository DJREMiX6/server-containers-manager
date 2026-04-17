using ServerContainerManager.API.Models.Responses.Common;
using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ResponsesExtensions
    {
        public static ContainerLabelResponseModel ToResponseModel(this ContainerLabel label) => new()
        {
            Key = label.Key,
            Value = label.Value
        };

        public static IReadOnlyCollection<ContainerLabelResponseModel> ToResponseModel(this IEnumerable<ContainerLabel> labels) => [.. labels.Select(ToResponseModel)];

        public static ContainerPortResponseModel ToResponseModel(this ContainerPort port) => new()
        {
            Public = port.Public,
            Private = port.Private,
        };

        public static IReadOnlyCollection<ContainerPortResponseModel> ToResponseModel(this IEnumerable<ContainerPort> ports) => [.. ports.Select(ToResponseModel)];

        public static NamespaceInfoResponseModel ToResponseModel(this NamespaceInfo namespaceInfo) => new()
        {
            Id = namespaceInfo.Id,
            Name = namespaceInfo.Name,
        };

        public static IReadOnlyCollection<NamespaceInfoResponseModel> ToResponseModel(this IEnumerable<NamespaceInfo> namespacesInfo) => [.. namespacesInfo.Select(ToResponseModel)];
    } 
}
