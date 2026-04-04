using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Responses.Common
{
    public sealed record ContainerPortResponseModel
    {
        public required uint Public { get; init; }
        public required uint Private { get; init; }

        public static ContainerPortResponseModel FromQueryModel(ContainerPort port) => new()
        {
            Public = port.Public,
            Private = port.Private,
        };
    }
}
