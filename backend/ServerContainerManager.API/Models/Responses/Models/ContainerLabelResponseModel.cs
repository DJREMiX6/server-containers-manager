using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Responses.Models
{
    public sealed record ContainerLabelResponseModel
    {
        public required string Key { get; init; }
        public required string Value { get; init; }

        public static ContainerLabelResponseModel FromQueryModel(ContainerLabel label) => new()
        {
            Key = label.Key,
            Value = label.Value
        };
    }
}
