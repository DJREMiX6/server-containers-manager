using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Queries.Abstraction;

namespace ServerContainerManager.Application.Queries.Namespace.GetNamespacesList
{
    internal class GetNamespacesListQueryHandler(
        ILogger<GetNamespacesListQueryHandler> logger,
        AppDbContext dbContext) : IQueryHandler<GetNamespacesListQuery, GetNamespacesListQueryResult>
    {
        private readonly ILogger<GetNamespacesListQueryHandler> _logger;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<GetNamespacesListQueryResult>> HandleAsync(GetNamespacesListQuery query, CancellationToken cancellationToken = default)
        {
            var namespaces = await _dbContext.Namespaces
                .Select(n => new GetNamespacesListQueryResultNamespace()
                    {
                        Id = n.Id,
                        Name = n.Name,
                        AssociatedUsersCount = n.AssociatedUsers.Count,
                        AssociatedContainersCount = _dbContext.Containers.Count(c => c.Namespaces.Contains(n)),
                    })
                .ToListAsync(cancellationToken);

            return new GetNamespacesListQueryResult()
            {
                Namespaces = namespaces,
                TotalCount = namespaces.Count
            };
        }
    }
}
