using ServerContainerManager.API.Models.Responses.Common;

namespace ServerContainerManager.API.Models.Responses.AuthController
{
    public sealed record GetSessionInfoResponse
    {
        public required UserResponseModel User { get; init; }
    }
}
