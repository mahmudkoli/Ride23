using Ride23.Framework.Core.Events;

namespace Ride23.Framework.Core.Messaging;

public interface IKafkaMessagePublisher<TEvent> where TEvent : IEvent
{
    Task PublishAsync(string key, TEvent @event, CancellationToken cancellationToken = default);
}