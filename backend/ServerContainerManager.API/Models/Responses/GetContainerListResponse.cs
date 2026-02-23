using ServerContainerManager.API.Models.Enums;

namespace ServerContainerManager.API.Models.Responses
{
    public record GetContainerListResponse(
        string Id,
        ContainerState Status,
        DateTime Created,
        IDictionary<string, string> Labels,
        string Name,
        IEnumerable<ushort> Ports);
}
