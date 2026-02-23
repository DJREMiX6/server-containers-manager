namespace ServerContainerManager.API.Models.Responses
{
    public sealed record SignInResponseNamespace(Guid Id, string Name);
    public sealed record SignInResponse(Guid Id, string Username, IEnumerable<string> Roles, IEnumerable<SignInResponseNamespace> Namespaces);
}
