namespace ServerContainerManager.API.Models.Requests.UsersController
{
    public sealed record GetUserListQueryParameters
    {
        public string? Username { get; init; }
    }
}
