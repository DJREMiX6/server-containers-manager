using ServerContainerManager.API.Models.Enums;

namespace ServerContainerManager.API.Models.Responses
{
    public record GetContainerListResponse
    {
        public string Id { get; set; }
        public ContainerState Status { get; set; }
        public DateTime Created { get; set; }
        public IDictionary<string, string> Labels { get; set; }
        public string Name { get; set; }
        public IEnumerable<ushort> Ports { get; set; }
    }
}
