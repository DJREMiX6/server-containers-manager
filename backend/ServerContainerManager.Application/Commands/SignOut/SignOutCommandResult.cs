using ErrorOr;

namespace ServerContainerManager.Application.Commands.SignOut
{
    public record SignOutCommandResult(bool IsError, List<Error> Errors);
}
