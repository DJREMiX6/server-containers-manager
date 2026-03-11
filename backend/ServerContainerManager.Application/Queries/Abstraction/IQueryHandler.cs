using ErrorOr;

namespace ServerContainerManager.Application.Queries.Abstraction
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : class
    {
        Task<ErrorOr<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
