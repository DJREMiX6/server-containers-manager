using ServerContainerManager.Domain.Entities.Containers.Enums;
using ServerContainerManager.Domain.Entities.Containers.ValueObjects;

namespace ServerContainerManager.API.Models.Responses.ContainersController
{
    public record GetContainerListItemResponse
    {
        public required string Id { get; init;  }
        public required string Name { get; init; }
        public required ContainerState State { get; init; }
        public required DateTime CreatedAt { get; init; }
        public required IReadOnlyCollection<Label> Labels { get; init; }
        public required IReadOnlyCollection<Port> Ports { get; init; }
    }

    public record GetContainerListResponse
    {
        public required IList<GetContainerListItemResponse> Projects { get; init; }
        public required int TotalCount { get; init;  }
    }
}
