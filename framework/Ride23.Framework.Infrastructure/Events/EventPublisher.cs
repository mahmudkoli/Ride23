using MediatR;
using Microsoft.Extensions.Logging;
using Ride23.Framework.Core.Events;

namespace Ride23.Framework.Infrastructure.Events;

public class EventPublisher : IEventPublisher
{
    private readonly IPublisher _publisher;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IPublisher publisher, ILogger<EventPublisher> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken token = default) where TEvent : IEvent
    {
        _logger.LogInformation("Publishing Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        return _publisher.Publish(CreateEventNotification(@event));
    }

    private INotification CreateEventNotification(IEvent @event) =>
        (INotification)Activator.CreateInstance(
            typeof(EventNotification<>).MakeGenericType(@event.GetType()), @event)!;
}
