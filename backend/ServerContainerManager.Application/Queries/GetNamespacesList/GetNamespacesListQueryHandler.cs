using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Application.Queries.GetContainerList;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Application.Queries.GetNamespacesList
{
    internal class GetNamespacesListQueryHandler(
        ILogger<GetNamespacesListQueryHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager) : IQueryHandler<GetNamespacesListQuery, GetNamespacesListQueryResult>
    {
        private readonly ILogger<GetNamespacesListQueryHandler> _logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetNamespacesListQueryResult>> HandleAsync(GetNamespacesListQuery query, CancellationToken cancellationToken = default)
        {
            var user = await GetUserAsync(query.UserId, cancellationToken);
            if (user.IsError)
                return user.Errors;

            var isUserAdmin = await _userManager.IsInRoleAsync(user.Value, UserRoles.Admin);
            var userNamespacesIds = user.Value.Namespaces.Select(n => n.Id);

            var namespacesQuery = _dbContext.Namespaces.AsQueryable();

            if (!isUserAdmin)
                namespacesQuery = namespacesQuery.ApplyFilter(userNamespacesIds);

            var namespaces = await namespacesQuery
                .Parse()
                .ToListAsync(cancellationToken);

            return new GetNamespacesListQueryResult()
            {
                Namespaces = namespaces,
                TotalCount = namespaces.Count
            };
        }

        private async Task<ErrorOr<AppUser>> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).Include(u => u.Namespaces).FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                return Error.Unauthorized($"{nameof(GetContainerListQueryHandler)}.{nameof(GetUserAsync)}", $"Cannot find user {userId}");

            return user;
        }
    }

    file static class GetNamespacesListQueryHandlerExtensions
    {
        public static IQueryable<Namespace> ApplyFilter(this IQueryable<Namespace> query, IEnumerable<Guid> namespacesIds) =>
            query.Where(n => namespacesIds.Contains(n.Id));

        public static IQueryable<NamespaceInfo> Parse(this IQueryable<Namespace> query) => 
            query.Select(n => NamespaceInfo.FromDomain(n));
    }
}
