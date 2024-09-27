using Microsoft.Extensions.Logging;
using Ride23.Event.Location;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Location.API.Events;
public class DriverLocationCreatedDomainEventHandler : EventNotificationHandler<DriverLocationCreatedDomainEvent>
{
    private readonly ILogger<DriverLocationCreatedDomainEventHandler> _logger;
    private readonly IKafkaMessagePublisher<DriverLocationCreatedIntegrationEvent> _messagePublisher;

    public DriverLocationCreatedDomainEventHandler(
        ILogger<DriverLocationCreatedDomainEventHandler> logger,
        IKafkaMessagePublisher<DriverLocationCreatedIntegrationEvent> messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public override Task Handle(DriverLocationCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        _messagePublisher.PublishAsync(@event.IdentityId, new DriverLocationCreatedIntegrationEvent(@event.IdentityId, @event.Latitude, @event.Longitude, @event.CellIndex, @event.Timestamp), cancellationToken);
        return Task.CompletedTask;
    }
}
