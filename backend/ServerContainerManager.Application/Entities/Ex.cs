using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServerContainerManager.Application.Entities
{
    internal static class Ex
    {
        internal static IServiceCollection RegisterDbs(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterAppDb(configuration);

            return services;
        }

        private static IServiceCollection RegisterAppDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("AppDb")));

            return services;
        }
    }
}
