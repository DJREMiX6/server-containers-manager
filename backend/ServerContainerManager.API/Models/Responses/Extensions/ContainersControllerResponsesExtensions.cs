using ServerContainerManager.API.Models.Responses.ContainersController;
using ServerContainerManager.Application.Queries.GetContainerList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ContainersControllerResponsesExtensions
    {
        public static GetContainerListItemResponse ToContract(this GetContainerListQueryResultContainerInfo result) => new()
        {
            Id = result.Id,
            State = result.State,
            CreatedAt = result.CreatedAt,
            Labels = result.Labels,
            Name = result.Name,
            Ports = result.Ports,
            Namespaces = result.Namespaces.ToResponseModel()
        };

        public static GetContainerListResponse ToContract(this GetContainerListQueryResult result) => new()
        {
            Containers = [.. result.Containers.Select(ToContract)],
            TotalCount = result.TotalCount,
        };
    }
}
