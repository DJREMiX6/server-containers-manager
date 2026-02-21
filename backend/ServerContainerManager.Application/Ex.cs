using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerContainerManager.Application.Commands;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Services;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application
{
    public static class Ex
    {
        public static IServiceCollection RegisterApplicationLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(typeof(Ex).Assembly);
            services.RegisterServices();
            services.RegisterCommands();
            services.RegisterDbs(configuration);
            services.RegisterIdentity();
            

            return services;
        }

        private static IServiceCollection RegisterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            return services;
        }
    }
}
