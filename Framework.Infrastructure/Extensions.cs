using Framework.Abstractions.Events;
using Framework.Abstractions.Exceptions;
using Framework.Abstractions.Repository;
using Framework.Infrastructure.Context;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Jobs;
using Framework.Infrastructure.Repository;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace Framework.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddFramework(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly assembly)
    {
        services.AddCommands(assembly);
        services.AddQueries(assembly);
        services.AddEvents(assembly);
        services.AddEventBus(configuration);
        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddErrorHandling();

        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services
            .AddDbContext<BaseDbContext>((sp, options) =>
            {
                options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection"))
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();

        var commandHandlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType
                          && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                              || i.GetGenericTypeDefinition()
                              == typeof(ICommandHandler<>))));

        // Register each query handler as scoped
        foreach (var type in commandHandlerTypes)
        {
            // Get the implemented interfaces for the query handler
            var interfaces = type.GetInterfaces()
                .Where(i =>
                    i.IsGenericType
                    &&
                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                     || i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

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

    private static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        return services
            .AddScoped<ErrorHandlerMiddleware>()
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
            .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();
    }

    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlerMiddleware>();
    }


    private static IServiceCollection AddEventBus(this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetSection("AppConfiguration:RabbitMQ").Get<RabbitMqOptions>();

        // Add the required Quartz.NET services
        services.AddQuartz(q =>
        {
            // Use a Scoped container to create jobs. I'll touch on this later
            q.UseMicrosoftDependencyInjectionScopedJobFactory();

            // Create a "key" for the job                    
            q.AddJobAndTrigger<OutboxJob>(configuration);
        });

        // Add the Quartz.NET hosted service

        services.AddQuartzHostedService(
            q => q.WaitForJobsToComplete = true);

        services.AddMassTransit(configurator =>
        {
            var eventConsumer = FindConsumers();

            // Register each EventConsumer
            foreach (var consumer in eventConsumer) configurator.AddConsumer(consumer);


            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config.Host,
                    //cfgSection["VirtualHost"],
                    h =>
                    {
                        h.Username(config.UserName);
                        h.Password(config.Password);
                    });

                // Enable the outbox

                // Register each EventConsumer

                foreach (var type in eventConsumer)
                    cfg.ReceiveEndpoint($"{type.FullName}",
                        c => { c.ConfigureConsumer(context, type); });
            });
        });

        return services;
    }


    // Get all classes implementing IConsumer<> from all loaded assemblies
    private static IEnumerable<Type> FindConsumers()
    {
        var consumerInterfaceType = typeof(IEventConsumer<>);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var consumer = new List<Type>();

        foreach (var assembly in assemblies)
            consumer.AddRange(assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => type.GetInterfaces()
                    .Any(interfaceType =>
                        interfaceType.IsGenericType &&
                        interfaceType.GetGenericTypeDefinition() == consumerInterfaceType)));

        return consumer;
    }

    private static void AddJobAndTrigger<T>(
        this IServiceCollectionQuartzConfigurator quartz,
        IConfiguration config)
        where T : IJob
    {
        // Use the name of the IJob as the appsettings.json key
        var jobName = typeof(T).Name;

        // Try and load the schedule from configuration
        var configKey = $"AppConfiguration:Quartz:{jobName}";
        var cronSchedule = config[configKey];

        // Some minor validation
        if (string.IsNullOrEmpty(cronSchedule))
            throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");

        // register the job as before
        var jobKey = new JobKey(jobName);
        quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithCronSchedule(cronSchedule)); // use the schedule from configuration
    }
}