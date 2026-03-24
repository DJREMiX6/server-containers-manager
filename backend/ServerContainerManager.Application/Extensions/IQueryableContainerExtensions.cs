using ServerContainerManager.Application.Models;
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
                (ContainerSortBy.Created, SortOrder.Asc) => query.OrderBy(c => c.CreatedAt),
                (ContainerSortBy.Created, SortOrder.Desc) => query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderBy(c => c.Name)
            };
    }
}
