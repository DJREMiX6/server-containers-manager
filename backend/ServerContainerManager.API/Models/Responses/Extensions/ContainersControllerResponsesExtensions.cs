using ServerContainerManager.API.Models.Enums;
using ServerContainerManager.API.Models.Responses.ContainersController;
using ServerContainerManager.Application.Commands.GetContainerList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ContainersControllerResponsesExtensions
    {
        public static GetContainerListResponse ToContract(this GetContainerListCommandResult response) => new (
            items: response.Containers
                .Select(c => new GetContainerListItemResponse(
                    id: c.Id,
                    state: ContainerStateHelper.FromDockerApiStatus(c.Status),
                    created: c.Created,
                    labels: c.Labels,
                    name: c.Name,
                    publicPorts: [.. c.PublicPorts]))
                .ToList());
    }
}
