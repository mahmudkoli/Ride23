using Microsoft.Extensions.Logging;
using Ride23.Inventory.Domain.Inventories.Events;
using Ride23.Event.Inventory;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Inventory.Application.Events;
public class InventoryCreatedDomainEventHandler : EventNotificationHandler<InventoryCreatedDomainEvent>
{
    private readonly ILogger<InventoryCreatedDomainEventHandler> _logger;
    private readonly IKafkaMessagePublisher<InventoryCreatedIntegrationEvent> _messagePublisher;

    public InventoryCreatedDomainEventHandler(
        ILogger<InventoryCreatedDomainEventHandler> logger, 
        IKafkaMessagePublisher<InventoryCreatedIntegrationEvent> messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public override Task Handle(InventoryCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        _messagePublisher.PublishAsync(@event.InventoryId.ToString(), new InventoryCreatedIntegrationEvent(@event.SupplierId, @event.InventoryId, @event.Amount, @event.Status.ToString()), cancellationToken);
        return Task.CompletedTask;
    }
}
