using ServerContainerManager.API.Models.Enums;

namespace ServerContainerManager.API.Models.Responses.ContainersController
{
    public record GetContainerListItemResponse
    {
        public required string Id { get; init;  }
        public required string Name { get; init; }
        public required ContainerState State { get; init; }
        public required DateTime Created { get; init; }
        public required IDictionary<string, string> Labels { get; init; }
        public required IList<ushort> PublicPorts { get; init; }
    }

    public record GetContainerListResponse
    {
        public required IList<GetContainerListItemResponse> Projects { get; init; }
        public required int TotalCount { get; init;  }
    }
}
