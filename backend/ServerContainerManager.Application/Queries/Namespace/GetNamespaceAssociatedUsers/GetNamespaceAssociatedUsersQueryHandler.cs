using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Queries.Namespace.GetNamespaceAssociatedUsers
{
    internal sealed class GetNamespaceAssociatedUsersQueryHandler(
        ILogger<GetNamespaceAssociatedUsersQueryHandler> logger, 
        AppDbContext dbContext) : IQueryHandler<GetNamespaceAssociatedUsersQuery, GetNamespaceAssociatedUsersQueryResult>
    {
        private readonly ILogger<GetNamespaceAssociatedUsersQueryHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<GetNamespaceAssociatedUsersQueryResult>> HandleAsync(GetNamespaceAssociatedUsersQuery query, CancellationToken cancellationToken = default)
        {
            if (!await _dbContext.Namespaces.AnyAsync(n => n.Id == query.NamespaceId, cancellationToken))
                return NamespaceErrors.NotFound(query.NamespaceId);

            var associatedUsers = await _dbContext.Users
                .Where(u => u.Namespaces
                    .Any(n => n.Id == query.NamespaceId))
                .Select(u => new NamespaceAssociatedUser()
                {
                    Id = u.Id,
                    Username = u.UserName!
                })
                .ToListAsync(cancellationToken);

            return new GetNamespaceAssociatedUsersQueryResult()
            {
                AssociatedUsers = [.. associatedUsers]
            };
        }
    }
}
