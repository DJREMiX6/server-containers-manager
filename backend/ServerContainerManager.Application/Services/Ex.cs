using Docker.DotNet;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Options;

namespace ServerContainerManager.Application.Services
{
    internal static class Ex
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.RegisterDockerService();

            return services; 
        }

        private static IServiceCollection RegisterDockerService(this IServiceCollection services)
        {
            services.AddFluentValidatedOptions<DockerOptions>(DockerOptions.SectionName);

            services.AddSingleton(sp =>
            {
                var dockerOptions = sp.GetRequiredService<IOptions<DockerOptions>>().Value;
                return new DockerClientConfiguration(new Uri(dockerOptions.Endpoint)).CreateClient();
            });

            return services;
        }
    }
}
