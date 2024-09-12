using Framework.Abstractions.Events;
using Framework.Abstractions.Outbox;
using Framework.Abstractions.Repository;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Framework.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class OutboxJob(
    ILogger<OutboxJob> logger,
    IOutboxRepository repository,
    IUnitOfWork unitOfWork,
    IEventDispatcher messagePublisher)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var readyToSendItems = await repository.GetAllReadyToSend();
        logger.LogInformation($"Outbox count {readyToSendItems.Count}:date : {DateTime.Now.ToLongTimeString()}");
        
        foreach (var item in readyToSendItems)
        {
            var eventMessage = item.RecreateMessage();

            messagePublisher.PublishIntegrationEventAsync(eventMessage);
           
            item.ChangeState(OutboxMessageState.SendToQueue);
        }
        
        await unitOfWork.CompleteAsync();
    }
}