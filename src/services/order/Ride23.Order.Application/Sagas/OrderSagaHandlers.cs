using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class OrderSagaHandlers :
        Saga<OrderProcessingSagaData>,
        IAmInitiatedBy<OrderCreatedEvent>,
        IHandleMessages<OrderCancelledEvent>,
        IHandleMessages<OrderProcessingSuccessEvent>,
        IHandleMessages<OrderProcessingCompleteEvent>
{
    private readonly IBus _bus;
    private readonly ILogger<OrderSagaHandlers> _logger;

    public OrderSagaHandlers(IBus bus, ILogger<OrderSagaHandlers> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderProcessingSagaData> config)
    {
        config.Correlate<OrderCreatedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<OrderCancelledEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<OrderProcessingSuccessEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<OrderProcessingCompleteEvent>(msg => msg.OrderId, data => data.OrderId);
    }

    public async Task Handle(OrderCreatedEvent message)
    {
        _logger.LogInformation("Handling OrderCreatedEvent for OrderId: {OrderId}", message.OrderId);

        // Handle order creation
        if (!IsNew) return;

        // Send the next event
        //await _bus.Send(new OrderProcessingSuccessEvent(message.OrderId));
        await _bus.Send(new ReserveInventoryCommand(message.OrderId));
        await _bus.Send(new ProcessPaymentCommand(message.OrderId));
        await _bus.Send(new SendNotificationCommand(message.OrderId));
        await _bus.Send(new ShipOrderCommand(message.OrderId));
    }

    public async Task Handle(OrderCancelledEvent message)
    {
        _logger.LogInformation("Handling OrderCancelledEvent for OrderId: {OrderId}", message.OrderId);

        // Handle order cancellation
        Data.Cancelled = true; // Update the Cancelled flag
        //MarkAsComplete(); // Mark the saga as complete for order cancellation

        // No need to send the next event since the saga is completed
    }

    public async Task Handle(OrderProcessingSuccessEvent message)
    {
        _logger.LogInformation("Handling OrderProcessingSuccessEvent for OrderId: {OrderId}", message.OrderId);

        // Handle order processing success

        // Send the next event
        await _bus.Send(new OrderProcessingCompleteEvent(message.OrderId));
    }

    public async Task Handle(OrderProcessingCompleteEvent message)
    {
        _logger.LogInformation("Handling OrderProcessingCompleteEvent for OrderId: {OrderId}", message.OrderId);

        // Handle order processing completion
        Data.Completed = true; // Update the Completed flag
        MarkAsComplete(); // Mark the saga as complete for order processing completion

        // No need to send the next event since the saga is completed
    }
}
