using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Entities.Extensions
{
    public static class IHostExtensions
    {
        public static async Task<IHost> InitializeDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var dbContext = services.GetRequiredService<AppDbContext>();

            var connectionString = configuration.GetConnectionString("AppDb");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'AppDb' is not configured.");

            EnsureSqliteDirectoryExists(connectionString);

            await dbContext.Database.MigrateAsync();

            await InitiallizeRolesAsync(services);

            return host;
        }

        public static async Task<IHost> CreateAdminUserIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            var adminUsers = await userManager.GetUsersInRoleAsync(UserRoles.Admin);
            if (adminUsers.Count > 0)
                return host;

            var adminUserCreateResult = AppUser.Create("Admin", []);
            if (adminUserCreateResult.IsError)
                throw new InvalidOperationException(string.Join('\n', adminUserCreateResult.Errors.Select(e => $"Code: {e.Code} Description: {e.Description}")));

            var adminPassword = "Admin123!";

            var createResult = await userManager.CreateAsync(adminUserCreateResult.Value, adminPassword);
            if (!createResult.Succeeded)
                throw new InvalidOperationException(string.Join('\n', createResult.Errors.Select(e => $"{e.Code}: {e.Description}")));

            var roleAssignResult = await userManager.AddToRolesAsync(adminUserCreateResult.Value, [UserRoles.Admin, UserRoles.Member]);
            if (!roleAssignResult.Succeeded)
                throw new InvalidOperationException(string.Join('\n', roleAssignResult.Errors.Select(e => $"{e.Code}: {e.Description}")));

            return host;
        }

        private static void EnsureSqliteDirectoryExists(string connectionString)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);

            if (string.IsNullOrWhiteSpace(builder.DataSource))
                throw new InvalidOperationException("Invalid SQLite DataSource.");

            var fullPath = Path.GetFullPath(builder.DataSource);
            var directory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static async Task InitiallizeRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new AppRole(UserRoles.Admin));

            if (!await roleManager.RoleExistsAsync(UserRoles.Member))
                await roleManager.CreateAsync(new AppRole(UserRoles.Member));
        }
    }
}
