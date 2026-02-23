namespace ServerContainerManager.API.Models.Responses
{
    public record GetUserListResponseNamespace(Guid Id, string Name);
    public record GetUserListResponse(
        Guid Id,
        string Username,
        IEnumerable<string> Roles,
        IEnumerable<GetUserListResponseNamespace> Namespaces);
}
