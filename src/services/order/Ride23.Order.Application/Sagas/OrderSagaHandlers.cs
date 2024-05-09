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

        if (!IsNew) return;

        //await _bus.Send(new OrderProcessingSuccessEvent(message.OrderId));
        await _bus.Send(new ReserveInventoryCommand(message.OrderId));
        await _bus.Send(new ProcessPaymentCommand(message.OrderId));
        await _bus.Send(new SendNotificationCommand(message.OrderId));
        await _bus.Send(new ShipOrderCommand(message.OrderId));
    }

    public async Task Handle(OrderCancelledEvent message)
    {
        _logger.LogInformation("Handling OrderCancelledEvent for OrderId: {OrderId}", message.OrderId);
        Data.Cancelled = true;
        TryComplete();
    }

    public async Task Handle(OrderProcessingSuccessEvent message)
    {
        _logger.LogInformation("Handling OrderProcessingSuccessEvent for OrderId: {OrderId}", message.OrderId);
        await _bus.Send(new OrderProcessingCompleteEvent(message.OrderId));
    }

    public async Task Handle(OrderProcessingCompleteEvent message)
    {
        _logger.LogInformation("Handling OrderProcessingCompleteEvent for OrderId: {OrderId}", message.OrderId);

        Data.Completed = true;
        TryComplete();
    }

    private void TryComplete()
    {
        if (Data.IsCompleted()) MarkAsComplete();
    }
}
