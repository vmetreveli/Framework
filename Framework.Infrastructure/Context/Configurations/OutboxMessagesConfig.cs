using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Infrastructure.Context.Configurations;

public class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(c => c.Data);
        builder.Property(c => c.State);
        builder.Property(c => c.Type);

        builder.Property(e => e.EventId);

        builder.Property(c => c.EventDate);
        builder.Property(c => c.ModifiedDate);
    }
}