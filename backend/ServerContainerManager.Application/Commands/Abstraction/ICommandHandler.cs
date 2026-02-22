namespace ServerContainerManager.Application.Commands.Abstraction
{
    public interface ICommandHandler<TCommand, TResult> 
        where TCommand : class 
        where TResult : class
    {
        public Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
