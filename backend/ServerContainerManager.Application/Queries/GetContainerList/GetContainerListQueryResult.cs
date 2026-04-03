using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Models.Enums;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Queries.GetContainerList
{
    public record GetContainerListQueryResultContainerInfo
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public required ContainerState State { get; init; }
        public required DateTime CreatedAt { get; init; }
        public required DateTime UpdatedAt { get; init; }
        public required IReadOnlyCollection<ContainerLabel> Labels { get; init; }
        public required IReadOnlyCollection<ContainerPort> Ports { get; init; }
        public required IReadOnlyCollection<NamespaceInfo> Namespaces { get; init; }

        public static GetContainerListQueryResultContainerInfo FromDomain(Container container, IReadOnlyCollection<NamespaceInfo> namespaces) => new ()
        {
            Id = container.Id,
            Name = container.Name,
            State = ContainerStateHelper.FromDomain(container.State),
            CreatedAt = container.Created.At,
            UpdatedAt = container.Updated.At,
            Labels = [.. container.Labels.Select(ContainerLabel.FromDomain)],
            Ports = [.. container.Ports.Select(ContainerPort.FromDomain)],
            Namespaces = namespaces
        };
    }

    public record GetContainerListQueryResult
    {
        public required IReadOnlyList<GetContainerListQueryResultContainerInfo> Containers { get; init; }
        public required int TotalCount { get; init; }
    }
}
