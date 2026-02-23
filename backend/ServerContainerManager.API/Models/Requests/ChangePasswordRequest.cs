namespace ServerContainerManager.API.Models.Requests
{
    public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
