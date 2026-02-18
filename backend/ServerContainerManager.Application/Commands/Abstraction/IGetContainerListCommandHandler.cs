namespace ServerContainerManager.Application.Commands.Abstraction
{
    public interface IGetContainerListCommandHandler
    {
        public Task<IEnumerable<GetContainerListCommandResult>> HandleAsync(GetContainerListCommand command, CancellationToken cancellationToken = default);
    }
}
