namespace ServerContainerManager.API.Models.Responses.Common
{
    public record NamespaceInfoResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
