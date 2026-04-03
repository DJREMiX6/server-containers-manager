namespace ServerContainerManager.Application.Extensions
{
    internal static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int skip, int take) =>
            query.Skip(skip).Take(take);
    }
}
