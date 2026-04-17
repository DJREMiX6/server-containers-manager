namespace ServerContainerManager.API.Models.Responses.Common
{
    public sealed record UserResponseModel
    {
        public required Guid UserId { get; init; }
        public required string Username { get; init; }
        public required IList<string> Roles { get; init; }
        public required IList<NamespaceInfoResponseModel> Namespaces { get; init; }
        public required bool IsConfirmed { get; init; }
    }
}
