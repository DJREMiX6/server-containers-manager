using Docker.DotNet;
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
            services.RegisterDockerReconciliationServices();

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

        private static IServiceCollection RegisterDockerReconciliationServices(this IServiceCollection services)
        {
            services.AddFluentValidatedOptions<DockerContainersReconciliationOptions>(DockerContainersReconciliationOptions.SectionName);
            services.AddHostedService<DockerContainersEventsListenerService>();
            services.AddSingleton<DockerContainersEventsSignalsQueue>();
            services.AddHostedService<PeriodicContainersReconciliator>();
            services.AddHostedService<DockerContainersEventsSignalsProcessor>();
            services.AddSingleton<DockerContainersReconciliator>();

            return services;
        }
    }
}
