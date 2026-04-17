using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Responses.Common
{
    public sealed record ContainerLabelResponseModel
    {
        public required string Key { get; init; }
        public required string Value { get; init; }
    }
}
