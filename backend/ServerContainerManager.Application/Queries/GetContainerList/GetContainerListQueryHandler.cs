using Docker.DotNet;
using Docker.DotNet.Models;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Application.Queries.GetContainerList
{
    internal class GetContainerListQueryHandler(
        ILogger<GetContainerListQueryHandler> logger,
        DockerClient dockerClient,
        UserManager<AppUser> userManager) : IQueryHandler<GetContainerListQuery, GetContainerListQueryResult>
    {
        private readonly ILogger<GetContainerListQueryHandler> _logger = logger;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetContainerListQueryResult>> HandleAsync(GetContainerListQuery query, CancellationToken cancellationToken = default)
        {
            var user = await GetUserAsync(query.UserId, cancellationToken);
            if (user.IsError)
                return user.Errors;

            if (await _userManager.IsInRoleAsync(user.Value, UserRoles.Admin))
                return await GetContainersForAdminUser(query.Skip, query.Take, query.SortBy, query.Order, cancellationToken);
            else
                return await GetContainersForMemberUser(query.Skip, query.Take, query.SortBy, query.Order, [.. user.Value.Namespaces], cancellationToken);
        }

        private async Task<ErrorOr<AppUser>> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).Include(u => u.Namespaces).FirstOrDefaultAsync(cancellationToken);
            if(user == null)
                return Error.Unauthorized($"{nameof(GetContainerListQueryHandler)}.{nameof(GetUserAsync)}", $"Cannot find user {userId}");

            return user;
        }

        private async Task<GetContainerListQueryResult> GetContainersForAdminUser(int skip, int take, ContainerSortBy sortBy, SortOrder order, CancellationToken cancellationToken)
        {
            var allContainers = await GetAllContainers(cancellationToken);
            var sorted = ApplySorting(allContainers, sortBy, order);
            var totalCount = sorted.Count;
            var paged = ApplyPaging(sorted, skip, take);
            var parsed = Parse(paged);

            return new GetContainerListQueryResult
            {
                Containers = parsed,
                TotalCount = totalCount
            };
        }

        private async Task<GetContainerListQueryResult> GetContainersForMemberUser(int skip, int take, ContainerSortBy sortBy, SortOrder order, IList<Namespace> namespaces, CancellationToken cancellationToken)
        {
            var allContainers = await GetAllContainers(cancellationToken);
            var sorted = ApplySorting(allContainers, sortBy, order);
            var filtered = FilterByNamespaces(sorted, namespaces);
            var totalCount = sorted.Count;
            var paged = ApplyPaging(filtered, skip, take);
            var parsed = Parse(paged);

            return new GetContainerListQueryResult
            {
                Containers = parsed,
                TotalCount = totalCount
            };
        }

        private async Task<IList<ContainerListResponse>> GetAllContainers(CancellationToken cancellationToken) => 
            await _dockerClient
            .Containers
            .ListContainersAsync(new ContainersListParameters() { All = true }, cancellationToken);

        private static List<ContainerListResponse> ApplySorting(
            IList<ContainerListResponse> containers,
            ContainerSortBy sortBy,
            SortOrder order) => (sortBy, order) switch
            {
                (ContainerSortBy.Name, SortOrder.Asc) => [.. containers.OrderBy(c => c.Names[0])],
                (ContainerSortBy.Name, SortOrder.Desc) => [.. containers.OrderByDescending(c => c.Names[0])],
                (ContainerSortBy.Status, SortOrder.Asc) => [.. containers.OrderBy(c => c.State)],
                (ContainerSortBy.Status, SortOrder.Desc) => [.. containers.OrderByDescending(c => c.State)],
                (ContainerSortBy.Created, SortOrder.Asc) => [.. containers.OrderBy(c => c.Created)],
                (ContainerSortBy.Created, SortOrder.Desc) => [.. containers.OrderByDescending(c => c.Created)],
                _ => [.. containers.OrderBy(c => c.Names[0])]
            };

        private static List<ContainerListResponse> FilterByNamespaces(IList<ContainerListResponse> containers, IList<Namespace> namespaces) => 
            containers
            .Where(c => namespaces
                .Any(n => c.Labels
                    .Contains(KeyValuePair
                        .Create(ContainersConsts.LabelNamespacePrefix, n.Id.ToString()))))
            .ToList();

        private static List<ContainerListResponse> ApplyPaging(IList<ContainerListResponse> containers, int skip, int take) =>
            containers
            .Skip(skip)
            .Take(take)
            .ToList();

        private static List<GetContainerListQueryResultContainerInfo> Parse(IList<ContainerListResponse> containers) => 
            containers
            .Select(c => new GetContainerListQueryResultContainerInfo
            {
                Id = c.ID,
                Name = c.Names[0],
                Status = c.State,
                Created = c.Created,
                Labels = c.Labels,
                PrivatePorts = [.. c.Ports.Select(p => p.PrivatePort)],
                PublicPorts = [.. c.Ports.Select(p => p.PublicPort)]
            })
            .ToList();
    }
}
