using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServerContainerManager.Application.Commands;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Services;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application
{
    public static class Ex
    {
        public static IServiceCollection RegisterApplicationLayerServices(
            this IServiceCollection services, 
            Action<DbContextOptionsBuilder> appDbOptionsBuilder)
        {
            services.AddValidatorsFromAssembly(typeof(Ex).Assembly);
            services.RegisterServices();
            services.RegisterCommandsFromAssembly(typeof(Ex).Assembly);
            services.RegisterDbs(appDbOptionsBuilder);
            services.RegisterIdentity();            

            return services;
        }

        private static IServiceCollection RegisterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
