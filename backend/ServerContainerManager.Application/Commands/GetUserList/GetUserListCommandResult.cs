namespace ServerContainerManager.Application.Commands.GetUserList
{
    public sealed record GetUserListCommandResultNamespace(Guid Id, string Name);
    public sealed record GetUserListCommandResult(
        Guid Id,
        string Username,
        IEnumerable<string> Roles,
        IEnumerable<GetUserListCommandResultNamespace> Namespaces);
}
