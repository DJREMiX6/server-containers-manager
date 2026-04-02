using ServerContainerManager.Domain.Entities.Containers.ValueObjects;

namespace ServerContainerManager.Application.Models
{
    public class ContainerLabel
    {
        public required string Key { get; init; }
        public required string Value { get; init; }

        public static ContainerLabel FromDomain(Label lael) => 
            new () { Key = lael.Key, Value = lael.Value };
    }
}
