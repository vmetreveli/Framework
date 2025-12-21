using Meadow_Framework.Framework.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Meadow_Framework.Framework.Infrastructure;

/// <summary>
///
/// </summary>
public static class MigrationExtensions
{
    public static  WebApplication ApplyMigration(this WebApplication app)
    {
        // Create a scope to retrieve services
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<MigrationExtensionsLogger>>();

        // Find all DbContext types implementing IDbContext
        var dbContextTypes = FindDbContexts();

        foreach (var dbContextType in dbContextTypes)
        {
            try
            {
                logger.LogInformation("Starting migration for {DbContextName}...", dbContextType.Name);

                // Resolve the DbContext and apply migrations

                var context = (DbContext)services.GetRequiredService(dbContextType);
                // if (context.Database.GetPendingMigrations().Any())
                //  {
                //      context.Database.Migrate();
                //  }

                if (context.Database.IsNpgsql())
                {
                    var pendingMigrations =  context.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                         context.Database.Migrate();
                    }
                }

                logger.LogInformation("Migration completed for {DbContextName}.", dbContextType.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database for {DbContextName}.",
                    dbContextType.Name);
            }
        }

        return app;
    }

    private static IEnumerable<Type> FindDbContexts()
    {
        // IDbContext is used to identify relevant DbContext types
        Type dbContextInterface = typeof(IDbContext);
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Retrieve all classes that implement IDbContext and are DbContext
        var dbContexts = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => !type.IsAbstract && typeof(DbContext).IsAssignableFrom(type))
            .Where(type => type.GetInterfaces().Contains(dbContextInterface));

        return dbContexts;
    }
}

/// <summary>
///
/// </summary>
public class MigrationExtensionsLogger
{
}