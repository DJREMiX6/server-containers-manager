using ServerContainerManager.API.Models.Enums;
using ServerContainerManager.API.Models.Responses.ContainersController;
using ServerContainerManager.Application.Queries.GetContainerList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ContainersControllerResponsesExtensions
    {
        public static GetContainerListResponse ToContract(this GetContainerListQueryResult result) => new()
        {
            Projects = result.Containers
                .Select(c => new GetContainerListItemResponse()
                {
                    Id = c.Id,
                    State = ContainerStateHelper.FromDockerApiStatus(c.Status),
                    Created = c.Created,
                    Labels = c.Labels,
                    Name = c.Name,
                    PublicPorts = [.. c.PublicPorts]
                })
                .ToList(),
            TotalCount = result.TotalCount,
        };
    }
}
