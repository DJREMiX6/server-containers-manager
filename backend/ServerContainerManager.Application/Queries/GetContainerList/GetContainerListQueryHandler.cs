using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Models;
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
            var user = await GetUserAsync(query.UserId, cancellationToken);
            if (user.IsError)
                return user.Errors;

            var containersQuery = _dbContext.Containers.AsQueryable();

            if (!await _userManager.IsInRoleAsync(user.Value, UserRoles.Admin))
                containersQuery = containersQuery.ApplyFiltering([.. user.Value.Namespaces.Select(n => n.Id)]);

            var totalCount = await containersQuery.CountAsync(cancellationToken);
            var containers = await containersQuery
                .ApplySorting(query.SortBy, query.Order)
                .ApplyPaging(query.Skip, query.Take)
                .Parse()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new GetContainerListQueryResult()
            {
                Containers = containers,
                TotalCount = totalCount
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

    file static class GetContainerListQueryExtensions
    {
        public static IQueryable<Container> ApplySorting(
            this IQueryable<Container> query,
            ContainerSortBy sortBy,
            SortOrder order) => (sortBy, order) switch
            {
                (ContainerSortBy.Name, SortOrder.Asc) => query.OrderBy(c => c.Name),
                (ContainerSortBy.Name, SortOrder.Desc) => query.OrderByDescending(c => c.Name),
                (ContainerSortBy.Status, SortOrder.Asc) => query.OrderBy(c => c.State),
                (ContainerSortBy.Status, SortOrder.Desc) => query.OrderByDescending(c => c.State),
                (ContainerSortBy.Created, SortOrder.Asc) => query.OrderBy(c => c.CreatedAt),
                (ContainerSortBy.Created, SortOrder.Desc) => query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderBy(c => c.Name)
            };

        public static IQueryable<Container> ApplyFiltering(this IQueryable<Container> query, IEnumerable<Guid> namespacesIds) =>
            query.Where(c => c.Namespaces.Any(n => namespacesIds.Contains(n.Id)));

        public static IQueryable<Container> ApplyPaging(this IQueryable<Container> query, int skip, int take) =>
            query.Skip(skip).Take(take);

        public static IQueryable<GetContainerListQueryResultContainerInfo> Parse(this IQueryable<Container> query) =>
            query
            .Select(c => new GetContainerListQueryResultContainerInfo
            {
                Id = c.Id,
                Name = c.Name,
                State = c.State,
                CreatedAt = c.CreatedAt,
                Labels = c.Labels,
                Ports = c.Ports,
            });
    }
}
