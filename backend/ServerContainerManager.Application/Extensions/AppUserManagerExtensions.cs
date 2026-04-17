using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Extensions
{
    internal static class AppUserManagerExtensions
    {
        public static async Task<bool> UserExistsByIdAsync(this UserManager<AppUser> userManager, Guid userId, CancellationToken cancellationToken = default) =>
            await userManager.Users.Where(u => u.Id == userId).AnyAsync(cancellationToken);

        public static async Task<AppUser?> GetUserByIdAsync(this UserManager<AppUser> userManager, Guid userId, CancellationToken cancellationToken = default) =>
            await userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        public static async Task<AppUser?> GetUserByUsernameAsync(this UserManager<AppUser> userManager, string username, CancellationToken cancellationToken = default) =>
            await userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync(cancellationToken);

        public static async Task<AppUser?> GetUserWithNamespacesAsync(this UserManager<AppUser> userManager, Guid userId, CancellationToken cancellationToken = default) =>
            await userManager.Users.GetUserWithNamespacesAsync(userId, cancellationToken);
    }
}
