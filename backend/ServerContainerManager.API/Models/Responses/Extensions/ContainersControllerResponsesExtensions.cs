using ServerContainerManager.API.Models.Enums;
using ServerContainerManager.API.Models.Responses.ContainersController;
using ServerContainerManager.Application.Queries.Container.GetContainerList;

namespace ServerContainerManager.API.Models.Responses.Extensions
{
    public static class ContainersControllerResponsesExtensions
    {
        public static GetContainerListItemResponse ToContract(this GetContainerListQueryResultContainerInfo result) => new()
        {
            Id = result.Id,
            Name = result.Name,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
            State = ContainerStateHelper.FromApplication(result.State),
            Labels = result.Labels.ToResponseModel(),
            Ports = result.Ports.ToResponseModel(),
            Namespaces = result.Namespaces.ToResponseModel()
        };

        public static GetContainerListResponse ToContract(this GetContainerListQueryResult result) => new()
        {
            Containers = [.. result.Containers.Select(ToContract)],
            TotalCount = result.TotalCount,
        };
    }
}
