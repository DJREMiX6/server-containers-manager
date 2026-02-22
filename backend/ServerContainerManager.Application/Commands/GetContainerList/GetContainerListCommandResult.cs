using Docker.DotNet.Models;

namespace ServerContainerManager.Application.Commands.GetContainerList
{
    public record GetContainerListCommandResult
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Status { get; init; }
        public DateTime Created { get; init; }
        public IDictionary<string, string> Labels { get; init; }
        public IEnumerable<ushort> PrivatePorts { get; init; }
        public IEnumerable<ushort> PublicPorts { get; init; }
    }
}
