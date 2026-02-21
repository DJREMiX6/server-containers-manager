using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServerContainerManager.Application.Entities
{
    internal static class Ex
    {
        internal static IServiceCollection RegisterDbs(this IServiceCollection services, Action<DbContextOptionsBuilder> appDbOptionsBuilder)
        {
            services.RegisterAppDb(appDbOptionsBuilder);

            return services;
        }

        private static IServiceCollection RegisterAppDb(this IServiceCollection services, Action<DbContextOptionsBuilder> appDbOptionsBuilder)
        {
            services.AddDbContext<AppDbContext>(appDbOptionsBuilder);

            return services;
        }
    }
}
