using Ride23.Framework.Core.Events;

namespace Ride23.Framework.Core.Messaging;

public interface IKafkaMessageHandler<TEvent> where TEvent : IEvent
{
    Task HandleAsync(string key, TEvent value);
}