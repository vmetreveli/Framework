using MassTransit;

namespace Framework.Abstractions.Events;

public interface IEventConsumer<T> : IConsumer<T> where T : class, IIntegrationEvent
{
}