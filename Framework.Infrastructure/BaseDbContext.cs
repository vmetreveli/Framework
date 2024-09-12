// using MassTransit;
// using MassTransit.EntityFrameworkCoreIntegration;
// using Microsoft.EntityFrameworkCore.Design;
//
// namespace Framework.Infrastructure;
//
// public class BaseDbContext(DbContextOptions<BaseDbContext> options) : SagaDbContext(options)
// {
//     // Implement the abstract Configurations property
//     protected override IEnumerable<ISagaClassMap> Configurations =>
//         // Return any saga mappings or leave empty if none
//         new List<ISagaClassMap>();
//
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);
//
//         modelBuilder.AddInboxStateEntity();
//         modelBuilder.AddOutboxMessageEntity();
//         modelBuilder.AddOutboxStateEntity();
//     }
//
//     public DbSet<OutboxMessage> OutboxMessages { get; set; }
// }
//
// public class BaseDbContextFactory : IDesignTimeDbContextFactory<BaseDbContext>
// {
//     public BaseDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
//         optionsBuilder
//             .UseNpgsql("DefaultConnection")
//             .UseSnakeCaseNamingConvention();
//
//         return new BaseDbContext(optionsBuilder.Options);
//     }
// }

