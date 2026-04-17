using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Application.Queries.Container.GetContainerList;
using ServerContainerManager.Application.Queries.Namespace.GetNamespacesList;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Queries.Namespace.GetNamespacesList
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
            var user = await _userManager.Users.GetUserWithNamespacesAsync(query.UserId, cancellationToken);
            if (user == null)
                return Error.Unauthorized($"{nameof(GetContainerListQueryHandler)}.{nameof(HandleAsync)}", $"Cannot find user {query.UserId}");

            var isUserAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            var userNamespacesIds = user.Namespaces.Select(n => n.Id);

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
    }

    file static class GetNamespacesListQueryHandlerExtensions
    {
        public static IQueryable<Domain.Entities.Namespaces.Namespace> ApplyFilter(this IQueryable<Domain.Entities.Namespaces.Namespace> query, IEnumerable<Guid> namespacesIds) =>
            query.Where(n => namespacesIds.Contains(n.Id));

        public static IQueryable<NamespaceInfo> Parse(this IQueryable<Domain.Entities.Namespaces.Namespace> query) => 
            query.Select(n => NamespaceInfo.FromDomain(n));
    }
}
