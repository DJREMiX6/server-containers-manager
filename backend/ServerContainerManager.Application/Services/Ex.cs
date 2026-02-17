using Docker.DotNet;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Options;
using ServerContainerManager.Application.Services.Abstraction;

namespace ServerContainerManager.Application.Services
{
    public static class Ex
    {
        public static IServiceCollection RegisterServerContainerManagerApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Ex).Assembly);
            services.RegisterDockerQueryService();

            return services; 
        }

        private static IServiceCollection RegisterDockerQueryService(this IServiceCollection services)
        {
            services.AddFluentValidatedOptions<DockerOptions>(DockerOptions.SectionName);

            services.AddSingleton(sp =>
            {
                var dockerOptions = sp.GetRequiredService<IOptions<DockerOptions>>().Value;
                return new DockerClientConfiguration(new Uri(dockerOptions.Endpoint)).CreateClient();
            });
            services.AddSingleton<IDockerQueryService, DockerQueryService>();

            return services;
        }
    }
}
