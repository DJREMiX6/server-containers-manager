using Docker.DotNet.Models;
using ServerContainerManager.API.Models.Enums;
using ServerContainerManager.Application.Commands;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ContainersControllerResponsesExtensions
    {
        public static GetContainerListResponse ToContract(this GetContainerListCommandResult response) => 
            new GetContainerListResponse()
            {
                Id = response.Id,
                Status = ContainerStateHelper.FromDockerApiStatus(response.Status),
                Created = response.Created,
                Labels = response.Labels,
                Name = response.Name,
                Ports = response.PublicPorts,
            };

        public static IEnumerable<GetContainerListResponse> ToContract(this IEnumerable<GetContainerListCommandResult> responses) =>
            responses.Select(ToContract);
    }
}
