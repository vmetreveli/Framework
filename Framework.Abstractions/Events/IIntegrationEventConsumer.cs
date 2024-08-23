using MassTransit;

namespace Framework.Abstractions.Events;

public interface IIntegrationEventConsumer<T>:IConsumer<T> where T : IntegrationBaseEvent
{
    
}