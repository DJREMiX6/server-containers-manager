namespace ServerContainerManager.Application.Extensions
{
    internal static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int skip, int take) =>
            query.Skip(skip).Take(take);

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int? skip, int? take)
        {
            if (take is null && skip is null)
                return query;
            else if (take is null && skip is not null)
                return query.Skip(skip.Value);
            else if (take is not null && skip is null)
                return query.Take(take.Value);
            else 
                return query.Paginate(skip!.Value, take!.Value);
        }
    }
}
