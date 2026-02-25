using ErrorOr;

namespace ServerContainerManager.Application.Commands.Abstraction
{
    public abstract record AbstractCommandResult
    {
        public bool IsError { get; init; }
        public List<Error> Errors { get; init; }

        protected AbstractCommandResult(List<Error> errors)
        {
            IsError = true;
            Errors = [.. errors];
        }

        protected AbstractCommandResult(Error error)
        {
            IsError = true;
            Errors = [error];
        }

        protected AbstractCommandResult() 
        {
            IsError = false;
            Errors = [];
        }
    }
}
