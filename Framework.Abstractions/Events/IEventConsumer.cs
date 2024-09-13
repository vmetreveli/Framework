using Framework.Abstractions.Primitives;
using MassTransit;

namespace Framework.Abstractions.Events;

public interface IEventConsumer<T> : IConsumer<T> where T :  IntegrationBaseEvent
{
}