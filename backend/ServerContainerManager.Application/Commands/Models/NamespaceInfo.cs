using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Application.Commands.Models
{
    public record NamespaceInfo
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }

        public static NamespaceInfo FromDomain(Namespace @namespace) => new()
        {
            Id = @namespace.Id,
            Name = @namespace.Name,
        };
    }
}
