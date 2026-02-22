using ErrorOr;

namespace ServerContainerManager.Application.Commands.CreateUser
{
    public record CreateUserCommandResult
    {
        public bool IsError { get; init; }
        public List<Error> Errors { get; init; }
        public Guid UserId { get; init; }

        public CreateUserCommandResult(List<Error> errors)
        {
            IsError = true;
            Errors = errors;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public CreateUserCommandResult(Guid userId)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            IsError = false;
            UserId = userId;
        }
    }
}
