namespace ServerContainerManager.Application.Models
{
    public sealed record User
    {
        public required Guid UserId { get; init; }
        public required string Username { get; init; }
        public required IList<string> Roles { get; init; }
        public required IList<NamespaceInfo> Namespaces { get; init; }
        public required bool IsConfirmed { get; init; }
    }
}
