using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ServerContainerManager.Application.Commands;
using ServerContainerManager.Application.Services;

namespace ServerContainerManager.Application
{
    public static class Ex
    {
        public static IServiceCollection RegisterApplicationLayerServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Ex).Assembly);
            services.RegisterServices();
            services.RegisterCommands();

            return services;
        }
    }
}
