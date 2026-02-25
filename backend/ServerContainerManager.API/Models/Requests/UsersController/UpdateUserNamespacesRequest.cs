namespace ServerContainerManager.API.Models.Requests.UsersController
{
    public sealed record UpdateUserNamespacesRequest(IList<Guid> NamespacesIds);
}
