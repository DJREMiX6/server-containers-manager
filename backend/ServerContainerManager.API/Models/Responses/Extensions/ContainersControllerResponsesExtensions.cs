using Docker.DotNet.Models;
using ServerContainerManager.API.Models.Enums;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ContainersControllerResponsesExtensions
    {
        private static IEnumerable<ushort> ToContainerResponsePorts(this IList<Port> ports) => ports.Select(p => p.PrivatePort);

        public static GetContainerListResponse ToGetContainerListResponse(this ContainerListResponse containerListResponse) => 
            new ()
            {
                Id = containerListResponse.ID,
                Status = ContainerStateHelper.FromDockerApiStatus(containerListResponse.State),
                Created = containerListResponse.Created,
                Labels = containerListResponse.Labels,
                Name = containerListResponse.Names[0],
                Ports = containerListResponse.Ports.ToContainerResponsePorts(),
            };

        public static IEnumerable<GetContainerListResponse> ToGetContainerListResponse(this IEnumerable<ContainerListResponse> containerListResponses) =>
            containerListResponses.Select(ToGetContainerListResponse);
    }
}
