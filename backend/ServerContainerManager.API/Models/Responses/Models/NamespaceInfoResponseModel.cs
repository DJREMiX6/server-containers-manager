using ServerContainerManager.Application.Commands.Models;

namespace ServerContainerManager.API.Models.Responses.Models
{
    public record NamespaceInfoResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }

        public static NamespaceInfoResponseModel FromQueryModel(NamespaceInfo queryModel) => new()
        {
            Id = queryModel.Id,
            Name = queryModel.Name,
        };
    }
}
