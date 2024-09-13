using Framework.Abstractions.Events;
using Framework.Abstractions.Outbox;
using Framework.Abstractions.Repository;
using Framework.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Framework.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class OutboxJob(
    IServiceProvider serviceProvider,
    IEventDispatcher messagePublisher)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var readyToSendItems = await dbContext.Set<OutboxMessage>()
                .Where(m => m.State == OutboxMessageState.ReadyToSend)
                .ToListAsync(context.CancellationToken);

            foreach (var eventMessage in readyToSendItems.Select(item => item.RecreateMessage()))
            {
                await messagePublisher.PublishIntegrationEventAsync(eventMessage);
            }

            // Optional: Update the state of outbox messages after processing
            // await dbContext.SaveChangesAsync(stoppingToken);
        }
    }


// protected override async Task ExecuteAsync(CancellationToken stoppingToken)
// {
//     // var readyToSendItems = await repository.GetAllReadyToSend();
//
//
//     var readyToSendItems = await dbContext.Set<OutboxMessage>()
//         .Where(m => m.State == OutboxMessageState.ReadyToSend)
//         .ToListAsync().ConfigureAwait(false);
//
//     foreach (var eventMessage in readyToSendItems.Select(item => item.RecreateMessage()))
//     {
//         await messagePublisher.PublishIntegrationEventAsync(eventMessage);
//     }
// }
}