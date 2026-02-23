using Docker.DotNet.Models;
using ServerContainerManager.API.Models.Enums;
using ServerContainerManager.Application.Commands.GetContainerList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ContainersControllerResponsesExtensions
    {
        public static IEnumerable<GetContainerListResponse> ToContract(this IEnumerable<GetContainerListCommandResult> responses) =>
            responses.Select(r => new GetContainerListResponse(
                Id: r.Id,
                Status: ContainerStateHelper.FromDockerApiStatus(r.Status),
                Created: r.Created,
                Labels: r.Labels,
                Name: r.Name,
                Ports: r.PublicPorts
            ));
    }
}
