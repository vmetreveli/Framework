namespace Framework.Infrastructure.Context;

public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
{
    #region Entities

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}

// public class BaseDbContextFactory : IDesignTimeDbContextFactory<BaseDbContext>
// {
//     public BaseDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
//         optionsBuilder
//             .UseNpgsql("DefaultConnection")
//             .UseCamelCaseNamingConvention();
//
//         return new BaseDbContext(optionsBuilder.Options);
//     }
// }