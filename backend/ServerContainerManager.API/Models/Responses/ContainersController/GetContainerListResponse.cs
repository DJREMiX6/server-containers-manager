using ServerContainerManager.API.Models.Enums;
using ServerContainerManager.API.Models.Responses.Common;

namespace ServerContainerManager.API.Models.Responses.ContainersController
{
    public record GetContainerListItemResponse
    {
        public required string Id { get; init;  }
        public required string Name { get; init; }
        public required ContainerState State { get; init; }
        public required DateTime CreatedAt { get; init; }
        public required DateTime UpdatedAt { get; init; }
        public required IReadOnlyCollection<ContainerLabelResponseModel> Labels { get; init; }
        public required IReadOnlyCollection<ContainerPortResponseModel> Ports { get; init; }
        public required IReadOnlyCollection<NamespaceInfoResponseModel> Namespaces { get; init; }
    }

    public record GetContainerListResponse
    {
        public required IReadOnlyCollection<GetContainerListItemResponse> Containers { get; init; }
        public required int TotalCount { get; init;  }
    }
}
