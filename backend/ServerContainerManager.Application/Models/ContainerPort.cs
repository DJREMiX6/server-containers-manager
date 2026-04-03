using ServerContainerManager.Domain.Entities.Containers.ValueObjects;

namespace ServerContainerManager.Application.Models
{
    public class ContainerPort
    {
        public required ushort Public { get; init; }
        public required ushort Private { get; init; }

        public static ContainerPort FromDomain(Port port) => 
            new () { Public = port.Public, Private = port.Private }; 
    }
}
