using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Queries.Namespace.GetNamespaceAssociatedContainers
{
    internal class GetNamespaceAssociatedContainersQueryHandler(
        ILogger<GetNamespaceAssociatedContainersQueryHandler> logger,
        AppDbContext dbContext) : IQueryHandler<GetNamespaceAssociatedContainersQuery, GetNamespaceAssociatedContainersQueryResult>
    {
        private readonly ILogger<GetNamespaceAssociatedContainersQueryHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<GetNamespaceAssociatedContainersQueryResult>> HandleAsync(GetNamespaceAssociatedContainersQuery query, CancellationToken cancellationToken = default)
        {
            var @namespace = await _dbContext.Namespaces
                .Where(n => n.Id == query.NamespaceId)
                .Include(n => n.AssociatedContainers)
                .FirstOrDefaultAsync(cancellationToken);
            if(@namespace is null) return NamespaceErrors.NotFound(query.NamespaceId);

            var containers = @namespace.AssociatedContainers.Select(ac => new AssociatedContainer() { Id = ac.Id, Name = ac.Name });

            return new GetNamespaceAssociatedContainersQueryResult() { AssociatedContainers = [.. containers] };
        }
    }
}
