using Microsoft.Extensions.DependencyInjection;
using ServerContainerManager.Application.Queries.Abstraction;
using System.Reflection;

namespace ServerContainerManager.Application.Queries
{
    internal static class Ex
    {
        internal static IServiceCollection RegisterQueriesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerInterfaceType = typeof(IQueryHandler<,>);

            var handlerTypes = assembly
                .GetTypes()
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsInterface)
                .Select(t => new
                {
                    Implementation = t,
                    Interfaces = t.GetInterfaces()
                })
                .SelectMany(t => t.Interfaces,
                    (t, i) => new { t.Implementation, Interface = i })
                .Where(x =>
                    x.Interface.IsGenericType
                    && x.Interface.GetGenericTypeDefinition() == handlerInterfaceType)
                .DistinctBy(x => x.Interface);

            foreach (var handler in handlerTypes)
                services.AddScoped(handler.Interface, handler.Implementation);

            return services;
        }
    }
}
