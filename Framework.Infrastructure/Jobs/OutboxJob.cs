using Framework.Abstractions.Events;
using Framework.Abstractions.Repository;
using Framework.Infrastructure.Repository;
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
        using var scope = serviceProvider.CreateScope();
        var requiredService = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        var readyToSendItems = await requiredService.GetAllReadyToSend();

        foreach (var eventMessage in readyToSendItems.Select(item => item.RecreateMessage()))
            await messagePublisher.PublishIntegrationEventAsync(eventMessage);
    }
}