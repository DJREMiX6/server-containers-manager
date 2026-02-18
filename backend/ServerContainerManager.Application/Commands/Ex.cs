using Microsoft.Extensions.DependencyInjection;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Handlers;

namespace ServerContainerManager.Application.Commands
{
    internal static class Ex
    {
        internal static IServiceCollection RegisterCommands(this IServiceCollection services)
        {
            services.AddScoped<IGetContainerListCommandHandler, GetContainerListCommandHandler>();

            return services;
        }
    }
}
