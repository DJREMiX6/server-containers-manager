using ServerContainerManager.Domain.Entities.Containers.Enums;
using ServerContainerManager.Domain.Entities.Containers.ValueObjects;

namespace ServerContainerManager.Application.Queries.GetContainerList
{
    public record GetContainerListQueryResultContainerInfo
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public required ContainerState State { get; init; }
        public required DateTime CreatedAt { get; init; }
        public required IReadOnlyCollection<Label> Labels { get; init; }
        public required IReadOnlyCollection<Port> Ports { get; init; }
    }

    public record GetContainerListQueryResult
    {
        public required IReadOnlyList<GetContainerListQueryResultContainerInfo> Containers { get; init; }
        public required int TotalCount { get; init; }
    }
}
