using Framework.Abstractions.Primitives.Types;
using Newtonsoft.Json;

namespace Framework.Infrastructure.Interceptors;

public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            _ = ConvertDomainEventsToOutboxMessages(eventData.Context);
            _ = ConvertDomainEventsToOutboxMessages(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static async Task<Task> ConvertDomainEventsToOutboxMessages(DbContext context)
    {
        var outboxMessages = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }), Guid.NewGuid(), DateTime.UtcNow))
            .ToList();

        await context.Set<OutboxMessage>()
            .AddRangeAsync(outboxMessages);
        return Task.CompletedTask;
    }
}