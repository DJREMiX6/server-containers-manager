using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Extensions
{
    internal static class IQueryableAppUserExtensions
    {
        public static async Task<AppUser?> GetUserWithNamespacesAsync(this IQueryable<AppUser> query, Guid userId, CancellationToken cancellationToken) => 
            await query.Where(u => u.Id == userId).Include(u => u.Namespaces).FirstOrDefaultAsync(cancellationToken);
    }
}
