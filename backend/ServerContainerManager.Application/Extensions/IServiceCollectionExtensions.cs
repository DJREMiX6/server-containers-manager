using Microsoft.Extensions.DependencyInjection;

namespace ServerContainerManager.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentValidatedOptions<TOptions>(this IServiceCollection services, string configurationSection) where TOptions : class
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(configurationSection)
                .ValidateWithFluentValidator()
                .ValidateOnStart();

            return services;
        } 
    }
}
