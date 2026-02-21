using Microsoft.Extensions.DependencyInjection;
using ServerContainerManager.Application.Commands.Abstraction;

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
