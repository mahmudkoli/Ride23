using Microsoft.Extensions.Logging;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Location.API.Events;
public class LocationCreatedDomainEventHandler : EventNotificationHandler<LocationCreatedDomainEvent>
{
    private readonly ILogger<LocationCreatedDomainEventHandler> _logger;
    private readonly IKafkaMessagePublisher<LocationCreatedIntegrationEvent> _messagePublisher;

    public LocationCreatedDomainEventHandler(
        ILogger<LocationCreatedDomainEventHandler> logger,
        IKafkaMessagePublisher<LocationCreatedIntegrationEvent> messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public override Task Handle(LocationCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        _messagePublisher.PublishAsync(@event.IdentityId, new LocationCreatedIntegrationEvent(@event.IdentityId, @event.Latitude, @event.Longitude), cancellationToken);
        return Task.CompletedTask;
    }
}
