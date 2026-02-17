using Docker.DotNet;
using FluentValidation;
using Microsoft.Extensions.Options;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Options;
using ServerContainerManager.API.Options.Validators;
using ServerContainerManager.API.Services.Abstraction;

namespace ServerContainerManager.API.Services
{
    public static class Ex
    {
        public static IServiceCollection RegisterServerContainerManagerServices(this IServiceCollection services)
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
