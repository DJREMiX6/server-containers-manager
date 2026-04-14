using Microsoft.Extensions.DependencyInjection;
using ServerContainerManager.Application.Commands.Abstraction;
using System.Reflection;

namespace ServerContainerManager.Application.Commands
{
    internal static class Ex
    {
        internal static IServiceCollection RegisterCommandsFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            // Capture the open generic type definition once.
            var handlerInterfaceType = typeof(IQueryHandler<,>);

            var handlerTypes = assembly
                .GetTypes()
                /* Extract only concrete implementation types.
                *  Abstract classes and interfaces cannot be instantiated
                *  and therefore cannot be registered in the container. */
                .Where(t => 
                    !t.IsAbstract
                    && !t.IsInterface)
                /* For each concrete type, project both:
                 * - The implementation type itself
                 * - All interfaces it implements
                 * This allows us to later match only ICommandHandler<,> interfaces. */
                .Select(t => new
                    {
                        Implementation = t,
                        Interfaces = t.GetInterfaces()
                    })
                /* Flatten the (Type x Interface[]) structure into individual (Implementation, Interface) pairs.
                 * Each pair represents a potential DI implementation. */
                .SelectMany(t => t.Interfaces,
                    (t, i) => new { t.Implementation, Interface = i })
                /* Filter only interfaces that:
                 * - Are generic (e.g. ICommandHandler<CreateUser, Result>) 
                 * - Have the generic type definition ICommandHandler<,>
                 * 
                 * Example of non-match:
                 * - IDisposable
                 * - INotificationHandler<>
                 */
                .Where(x => 
                    x.Interface.IsGenericType
                    && x.Interface.GetGenericTypeDefinition() == handlerInterfaceType)
                /* Defensive safeguard:
                 * If a class implements multiple interfaces that resolve 
                 * to the same closed ICommandHandler<,> (unlikely but possible),
                 * avoid duplicate DI registrations. */
                .DistinctBy(x => x.Interface);

            // Register each handler as Scoped.
            foreach (var handler in handlerTypes)
                services.AddScoped(handler.Interface, handler.Implementation);

            return services;
        }
    }
}
