using ErrorOr;

namespace ServerContainerManager.Application.Commands.ChangePassword
{
    public sealed record ChangePasswordCommandResult
    {
        public bool IsError { get; init; }
        public List<Error> Errors { get; init; }

        public ChangePasswordCommandResult(List<Error> errors)
        {
            IsError = true;
            Errors = [.. errors];
        }

        public ChangePasswordCommandResult() 
        {
            IsError = false;
        }
    }
}
