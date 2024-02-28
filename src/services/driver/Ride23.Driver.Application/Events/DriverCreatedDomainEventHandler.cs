using Microsoft.Extensions.Logging;
using Ride23.Driver.Domain.Drivers.Events;
using Ride23.Event.Driver;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Driver.Application.Events;
public class DriverCreatedDomainEventHandler : EventNotificationHandler<DriverCreatedDomainEvent>
{
    private readonly ILogger<DriverCreatedDomainEventHandler> _logger;
    private readonly IKafkaMessagePublisher<DriverCreatedIntegrationEvent> _messagePublisher;

    public DriverCreatedDomainEventHandler(
        ILogger<DriverCreatedDomainEventHandler> logger, 
        IKafkaMessagePublisher<DriverCreatedIntegrationEvent> messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public override Task Handle(DriverCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        _messagePublisher.PublishAsync(@event.DriverId.ToString(), new DriverCreatedIntegrationEvent(@event.IdentityId, @event.DriverId, @event.DriverName), cancellationToken);
        return Task.CompletedTask;
    }
}
