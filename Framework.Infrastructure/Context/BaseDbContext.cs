namespace Framework.Infrastructure.Context;

/// <summary>
/// BaseDbContext is the base database context class used to interact with the database.
/// It inherits from <see cref="DbContext"/> and provides configurations and DbSet definitions for entities.
/// </summary>
public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
{
    #region Entities

    /// <summary>
    /// DbSet representing the OutboxMessages table in the database.
    /// Used to store messages for the Outbox pattern to ensure reliable message delivery.
    /// </summary>
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    #endregion

    /// <summary>
    /// Configures the entity mappings and relationships using the model builder.
    /// Applies all configurations found in the current assembly.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to create and configure the entity model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Automatically apply entity configurations from the assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}