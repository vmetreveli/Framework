using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Infrastructure.Context.Configurations;

/// <summary>
///     Configuration class for the <see cref="OutboxMessage" /> entity.
///     Implements <see cref="IEntityTypeConfiguration{TEntity}" /> to configure the entity properties and keys.
/// </summary>
public class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
{
    /// <summary>
    ///     Configures the <see cref="OutboxMessage" /> entity's properties and keys using the provided
    ///     <see cref="EntityTypeBuilder{TEntity}" />.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder{OutboxMessage}" /> to configure the entity.</param>
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        // Configures the primary key for the OutboxMessage entity
        builder.HasKey(e => e.Id);

        // Configures properties for the entity
        builder.Property(c => c.Data); // Message payload (data)
        builder.Property(c => c.State); // Current state of the message (e.g., ReadyToSend, Sent, etc.)
        builder.Property(c => c.Type); // Message type (event type)

        builder.Property(e => e.EventId); // Unique identifier for the event

        builder.Property(c => c.EventDate); // Date when the event occurred
        builder.Property(c => c.ModifiedDate); // Date when the message was last modified
    }
}