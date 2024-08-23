using MassTransit;

namespace Framework.Infrastructure;

// public class BaseDbContext(DbContextOptions<DbContext> options) : DbContext(options)
// {
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);
//
//         modelBuilder.AddInboxStateEntity();
//         modelBuilder.AddOutboxMessageEntity();
//         modelBuilder.AddOutboxStateEntity();
//     }
// }