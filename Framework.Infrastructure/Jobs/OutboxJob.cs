using Framework.Abstractions.Events;
using Framework.Abstractions.Repository;
using Quartz;

namespace Framework.Infrastructure.Jobs;

/// <summary>
///     Represents a Quartz job responsible for processing and dispatching outbox messages.
///     The job ensures that messages are only dispatched once by using outbox messaging patterns.
/// </summary>
[DisallowConcurrentExecution] // Ensures the job does not run concurrently.
public class OutboxJob(
    IServiceProvider serviceProvider,
    IEventDispatcher messagePublisher) : IJob
{
    /// <summary>
    ///     Executes the outbox job, fetching all ready-to-send messages from the outbox and dispatching them via the event
    ///     dispatcher.
    /// </summary>
    /// <param name="context">The context in which the job is executed, containing runtime information.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Execute(IJobExecutionContext context)
    {
        // Create a scope for resolving dependencies (outbox repository).
        using var scope = serviceProvider.CreateScope();

        // Get the IOutboxRepository instance to interact with the outbox messages.
        var requiredService = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        // Retrieve all messages that are ready to be sent.
        var readyToSendItems = await requiredService.GetAllReadyToSend();

        // Recreate the integration event from each outbox message and publish it using the event dispatcher.
        foreach (var eventMessage in readyToSendItems.Select(item => item.RecreateMessage()))
            // Publish the recreated integration event asynchronously.
            await messagePublisher.PublishIntegrationEventAsync(eventMessage);
    }
}