using ErrorOr;

namespace ServerContainerManager.Application.Commands.SignIn
{
    public record SignInCommandResult(bool IsError, List<Error> Errors);
}
