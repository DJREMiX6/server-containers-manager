namespace ServerContainerManager.Application.Commands.ChangePassword
{
    public sealed record ChangePasswordCommand(Guid CallerUserId, Guid UserId, string CurrentPassword, string NewPassword);
}
