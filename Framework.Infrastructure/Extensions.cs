using Framework.Abstractions.Commands;
using Framework.Abstractions.Dispatchers;
using Framework.Abstractions.Events;
using Framework.Abstractions.Queries;

namespace Framework.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddFramework(this IServiceCollection services, Assembly assembly)
    {
        services.AddCommands(assembly);
        services.AddQueries(assembly);
        services.AddEvents(assembly);
        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();

        var commandHandlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

        // Register each query handler as scoped
        foreach (var type in commandHandlerTypes)
        {
            // Get the implemented interfaces for the query handler
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            // Register each interface with the corresponding implementation
            foreach (var interfaceType in interfaces) services.AddScoped(interfaceType, type);
        }

        return services;
    }


    private static IServiceCollection AddQueries(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();


        // Get all types implementing IQueryHandler<,>
        var queryHandlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

        // Register each query handler as scoped
        foreach (var type in queryHandlerTypes)
        {
            // Get the implemented interfaces for the query handler
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            // Register each interface with the corresponding implementation
            foreach (var interfaceType in interfaces) services.AddScoped(interfaceType, type);
        }

        return services;
    }

    private static IServiceCollection AddEvents(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IEventDispatcher, EventDispatcher>();

        // Get all types implementing IQueryHandler<,>
        var eventHandlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

        // Register each query handler as scoped
        foreach (var type in eventHandlerTypes)
        {
            // Get the implemented interfaces for the query handler
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

            // Register each interface with the corresponding implementation
            foreach (var interfaceType in interfaces) services.AddScoped(interfaceType, type);
        }

        return services;
    }
}