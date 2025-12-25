using MassTransit;
using MassTransit.Serialization;
using Meadow_Framework.Abstractions.Commands;
using Meadow_Framework.Abstractions.Dispatchers;
using Meadow_Framework.Abstractions.Events;
using Meadow_Framework.Abstractions.Exceptions;
using Meadow_Framework.Abstractions.Queries;
using Meadow_Framework.Abstractions.Repository;
using Meadow_Framework.Infrastructure.Commands;
using Meadow_Framework.Infrastructure.Context;
using Meadow_Framework.Infrastructure.Dispatchers;
using Meadow_Framework.Infrastructure.Events;
using Meadow_Framework.Infrastructure.Exceptions;
using Meadow_Framework.Infrastructure.Jobs;
using Meadow_Framework.Infrastructure.Queries.Dispatcher;
using Meadow_Framework.Infrastructure.Repository;
using Meadow_Framework.Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using Meadow_Framework.Infrastructure.Security;

namespace Meadow_Framework.Infrastructure;

/// <summary>
///
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Extension method to add the framework services to the <see cref="IServiceCollection" />.
    ///     This includes commands, queries, events, event bus, error handling, and database configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which the services will be added.</param>
    /// <param name="configuration">The application configuration used for settings like database connections.</param>
    /// <param name="assemblies"></param>
    /// <returns>The modified <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddFramework(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            services.AddCommands(assembly);
            services.AddQueries(assembly);
            services.AddEvents(assembly);
        }

        services.AddEventBus(configuration, assemblies);
        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddErrorHandling();

        services.AddScoped<IOutboxRepository, OutboxRepository>();

        // Configure the database context with PostgreSQL settings
        services
            .AddDbContext<BaseDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

        return services;
    }
    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
        where TContext : DbContext
    {
        MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

        SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();

        return app;
    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }

    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders) await seeder.SeedAllAsync();
    }

    /// <summary>
    ///     Adds command dispatching and command handlers to the <see cref="IServiceCollection" />.
    ///     Registers all types that implement <see cref="ICommandHandler{TCommand, TResult}" /> and
    ///     <see cref="ICommandHandler{TCommand}" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <param name="assembly">The assembly to scan for command handlers.</param>
    /// <returns>The modified <see cref="IServiceCollection" />.</returns>
    private static IServiceCollection AddCommands(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();

        // Get all types that implement ICommandHandler<>
        var commandHandlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType
                          && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                              || i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))));

        // Register each command handler as scoped
        foreach (var type in commandHandlerTypes)
        {
            var interfaces = type.GetInterfaces()
                .Where(i =>
                    i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                     || i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

            foreach (var interfaceType in interfaces) services.AddScoped(interfaceType, type);
        }

        return services;
    }

    /// <summary>
    ///     Adds query dispatching and query handlers to the <see cref="IServiceCollection" />.
    ///     Registers all types that implement <see cref="IQueryHandler{TQuery, TResult}" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <param name="assembly">The assembly to scan for query handlers.</param>
    /// <returns>The modified <see cref="IServiceCollection" />.</returns>
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
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            foreach (var interfaceType in interfaces) services.AddScoped(interfaceType, type);
        }

        return services;
    }

    /// <summary>
    ///     Adds event dispatching and event handlers to the <see cref="IServiceCollection" />.
    ///     Registers all types that implement <see cref="IEventHandler{TEvent}" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <param name="assembly">The assembly to scan for event handlers.</param>
    /// <returns>The modified <see cref="IServiceCollection" />.</returns>
    private static IServiceCollection AddEvents(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IEventDispatcher, EventDispatcher>();

        // Get all types implementing IEventHandler<>
        var eventHandlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

        // Register each event handler as scoped
        foreach (var type in eventHandlerTypes)
        {
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

            foreach (var interfaceType in interfaces) services.AddScoped(interfaceType, type);
        }

        return services;
    }

    /// <summary>
    ///     Adds error handling middleware and exception handling services.
    ///     Registers <see cref="ErrorHandlerMiddleware" />, <see cref="IExceptionToResponseMapper" />, and
    ///     <see cref="IExceptionCompositionRoot" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <returns>The modified <see cref="IServiceCollection" />.</returns>
    private static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        return services
            .AddScoped<ErrorHandlerMiddleware>() // Middleware for error handling
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>() // Maps exceptions to responses
            .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>(); // Exception handler root
    }

    /// <summary>
    ///     Extension method to add the error handling middleware to the application pipeline.
    ///     This middleware catches exceptions and returns proper responses.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
    /// <returns>The modified <see cref="IApplicationBuilder" />.</returns>
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlerMiddleware>();
    }

    /// <summary>
    ///     Adds and configures the EventBus using MassTransit and RabbitMQ.
    ///     Also configures Quartz for job scheduling and triggers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <param name="configuration">The configuration object for RabbitMQ and Quartz settings.</param>
    /// <param name="assemblies">The assemblies to scan for consumers.</param>
    /// <returns>The modified <see cref="IServiceCollection" />.</returns>
    private static IServiceCollection AddEventBus(this IServiceCollection services,
        IConfiguration configuration, IEnumerable<Assembly> assemblies)
    {
        var config = configuration.GetSection("AppConfiguration:RabbitMQ").Get<RabbitMqOptions>();
        var encryptionKey = configuration["AppConfiguration:RabbitMQ:EncryptionKey"];

        // Add the required Quartz.NET services
        services.AddQuartz(q =>
        {
            q.AddJobAndTrigger<OutboxJob>(configuration); // Register OutboxJob with trigger
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true); // Ensure jobs complete before shutdown

        services.AddMassTransit(configurator =>
        {
            var consumers = FindConsumers(assemblies).ToList();

            foreach (var consumer in consumers)
                configurator.AddConsumer(consumer);

            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config!.Host, h =>
                {
                    h.Username(config.UserName);
                    h.Password(config.Password);
                });

                // Configure message encryption if a key is provided
                if (!string.IsNullOrWhiteSpace(encryptionKey))
                {
                    using var sha = SHA256.Create();
                    var encryptionKeyBytes = sha.ComputeHash(
                        Encoding.UTF8.GetBytes(encryptionKey)
                    );

                    cfg.ConfigureJsonSerializerOptions(options =>
                    {
                        options.TypeInfoResolver ??= new DefaultJsonTypeInfoResolver();

                        options.TypeInfoResolver = options.TypeInfoResolver.WithAddedModifier(typeInfo =>
                        {
                            foreach (var property in typeInfo.Properties)
                            {
                                if (property.PropertyType == typeof(string) &&
                                    property.AttributeProvider?.IsDefined(
                                        typeof(SensitiveDataAttribute), inherit: false) == true)
                                {
                                    property.CustomConverter =
                                        new EncryptedStringConverter(encryptionKeyBytes);
                                }
                            }
                        });

                        return options;
                    });
                }

                // Configure receive endpoints
                foreach (var consumerType in consumers)
                {
                    cfg.ReceiveEndpoint(consumerType.Name, endpoint =>
                    {
                        endpoint.ConfigureConsumer(context, consumerType);
                    });
                }
            });
        });


        return services;
    }

    /// <summary>
    ///     Finds and returns all classes that implement <see cref="IEventConsumer{TEvent}" /> from all loaded assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>A collection of consumer types implementing <see cref="IEventConsumer{TEvent}" />.</returns>
    private static IEnumerable<Type> FindConsumers(IEnumerable<Assembly> assemblies)
    {
        var consumerInterfaceType = typeof(IEventConsumer<>);
        var consumer = new List<Type>();

        // Search for classes implementing IEventConsumer<> in loaded assemblies
        foreach (var assembly in assemblies)
            consumer.AddRange(assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => type.GetInterfaces()
                    .ToList()
                    .Exists(interfaceType =>
                        interfaceType.IsGenericType &&
                        interfaceType.GetGenericTypeDefinition() == consumerInterfaceType)));

        return consumer;
    }

    /// <summary>
    ///     Adds and schedules a Quartz job using a cron schedule from configuration.
    /// </summary>
    /// <typeparam name="T">The type of the Quartz job to schedule.</typeparam>
    /// <param name="quartz">The Quartz.NET configuration object.</param>
    /// <param name="config">The configuration object used to retrieve the cron schedule.</param>
    /// <exception cref="Exception"></exception>
    private static void AddJobAndTrigger<T>(
        this IServiceCollectionQuartzConfigurator quartz,
        IConfiguration config)
        where T : IJob
    {
        var jobName = typeof(T).Name;
        var configKey = $"AppConfiguration:Quartz:{jobName}";
        var cronSchedule = config[configKey];

        // Validate that the cron schedule exists
        if (string.IsNullOrEmpty(cronSchedule))
            throw new FrameworkException($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");

        var jobKey = new JobKey(jobName);

        // Register the job and trigger using the cron schedule from configuration
        quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));
        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithCronSchedule(cronSchedule));
    }
}