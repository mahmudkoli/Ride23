using Microsoft.Extensions.Logging;
using Ride23.Order.Domain.Orders.Events;
using Ride23.Event.Order;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Order.Application.Events;
public class OrderCreatedDomainEventHandler : EventNotificationHandler<OrderCreatedDomainEvent>
{
    private readonly ILogger<OrderCreatedDomainEventHandler> _logger;
    private readonly IKafkaMessagePublisher<OrderCreatedIntegrationEvent> _messagePublisher;

    public OrderCreatedDomainEventHandler(
        ILogger<OrderCreatedDomainEventHandler> logger, 
        IKafkaMessagePublisher<OrderCreatedIntegrationEvent> messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public override Task Handle(OrderCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        _messagePublisher.PublishAsync(@event.OrderId.ToString(), new OrderCreatedIntegrationEvent(@event.CustomerId, @event.OrderId, @event.Amount, @event.Status.ToString()), cancellationToken);
        return Task.CompletedTask;
    }
}
