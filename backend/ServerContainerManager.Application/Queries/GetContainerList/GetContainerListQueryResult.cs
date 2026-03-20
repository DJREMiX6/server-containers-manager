using ServerContainerManager.Application.Models;
using ServerContainerManager.Domain.Entities.Containers;
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
        public required IReadOnlyCollection<NamespaceInfo> Namespaces { get; init; }

        public static GetContainerListQueryResultContainerInfo FromDomain(Container container, IReadOnlyCollection<NamespaceInfo> namespaces) => new ()
        {
            Id = container.Id,
            Name = container.Name,
            State = container.State,
            CreatedAt = container.CreatedAt,
            Labels = container.Labels,
            Ports = container.Ports,
            Namespaces = namespaces
        };
    }

    public record GetContainerListQueryResult
    {
        public required IReadOnlyList<GetContainerListQueryResultContainerInfo> Containers { get; init; }
        public required int TotalCount { get; init; }
    }
}
