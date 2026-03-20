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
                    State = c.State,
                    CreatedAt = c.CreatedAt,
                    Labels = c.Labels,
                    Name = c.Name,
                    Ports = [.. c.Ports],
                    Namespaces = c.Namespaces.ToResponseModel().AsReadOnly()
                })
                .ToList(),
            TotalCount = result.TotalCount,
        };
    }
}
