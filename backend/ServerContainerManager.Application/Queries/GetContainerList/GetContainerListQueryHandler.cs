using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Queries.GetContainerList
{
    internal class GetContainerListQueryHandler(
        ILogger<GetContainerListQueryHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager) : IQueryHandler<GetContainerListQuery, GetContainerListQueryResult>
    {
        private readonly ILogger<GetContainerListQueryHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetContainerListQueryResult>> HandleAsync(GetContainerListQuery query, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users.GetUserWithNamespacesAsync(query.UserId, cancellationToken);
            if (user == null)
                return Error.Unauthorized($"{nameof(GetContainerListQueryHandler)}.{nameof(HandleAsync)}", $"Cannot find user {query.UserId}");

            var isUserAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

            var namespacesIds = (isUserAdmin 
                ? await dbContext.Namespaces.Select(n => n.Id).ToListAsync(cancellationToken) 
                : user.Namespaces.Select(n => n.Id))
                .ToList();
            IQueryable<Container> containersQuery = _dbContext.Containers
                .AsNoTracking();
                
            if(!isUserAdmin)
                containersQuery = containersQuery.FilterByNamespaces(namespacesIds);

            var totalCount = await containersQuery.CountAsync(cancellationToken);
            var containers = await containersQuery
                .Sort(query.SortBy, query.Order)
                .Paginate(query.Skip, query.Take)
                .Parse(namespacesIds)
                .ToListAsync(cancellationToken);

            return new GetContainerListQueryResult()
            {
                Containers = containers,
                TotalCount = totalCount
            };
        }
    }
}
