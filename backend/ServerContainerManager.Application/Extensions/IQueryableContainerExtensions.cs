using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Queries.Container.GetContainerList;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Extensions
{
    internal static class IQueryableContainerExtensions
    {
        public static IQueryable<Container> FilterByNamespaces(this IQueryable<Container> query, IList<Guid> namespacesIds) =>
            query.Where(c => c.Namespaces.Any(n => namespacesIds.Contains(n.Id)));

        public static IQueryable<Container> Sort(
            this IQueryable<Container> query,
            ContainerSortBy sortBy,
            SortOrder order) => (sortBy, order) switch
            {
                (ContainerSortBy.Name, SortOrder.Asc) => query.OrderBy(c => c.Name),
                (ContainerSortBy.Name, SortOrder.Desc) => query.OrderByDescending(c => c.Name),
                (ContainerSortBy.Status, SortOrder.Asc) => query.OrderBy(c => c.State),
                (ContainerSortBy.Status, SortOrder.Desc) => query.OrderByDescending(c => c.State),
                (ContainerSortBy.Created, SortOrder.Asc) => query.OrderBy(c => c.Created.At),
                (ContainerSortBy.Created, SortOrder.Desc) => query.OrderByDescending(c => c.Created.At),
                (ContainerSortBy.Updated, SortOrder.Asc) => query.OrderBy(c => c.Updated.At),
                (ContainerSortBy.Updated, SortOrder.Desc) => query.OrderByDescending(c => c.Updated.At),
                _ => query.OrderBy(c => c.Name)
            };

        public static IQueryable<Container> Sort(
            this IQueryable<Container> query,
            ContainerSortBy? sortBy,
            SortOrder? order) => query.Sort(sortBy ?? ContainerSortBy.Name, order ?? SortOrder.Desc);

        public static IQueryable<GetContainerListQueryResultContainerInfo> Parse(this IQueryable<Container> query, IEnumerable<Guid> namespacesIds) =>
            query.Select(container => GetContainerListQueryResultContainerInfo.FromDomain(
                container,
                container.Namespaces
                .Select(n => NamespaceInfo.FromDomain(n))
                .ToList()));
    }
}
