using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class ShippingSagaHandlers :
        Saga<OrderProcessingSagaData>,
        IHandleMessages<OrderShippedEvent>,
        IHandleMessages<ShippingFailedEvent>
{
    private readonly IBus _bus;
    private readonly ILogger<ShippingSagaHandlers> _logger;

    public ShippingSagaHandlers(IBus bus, ILogger<ShippingSagaHandlers> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderProcessingSagaData> config)
    {
        config.Correlate<OrderShippedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<ShippingFailedEvent>(msg => msg.OrderId, data => data.OrderId);
    }

    public async Task Handle(OrderShippedEvent message)
    {
        _logger.LogInformation("Handling OrderShippedEvent for OrderId: {OrderId}", message.OrderId);
        Data.Shipped = true;
        TryComplete();
    }

    public async Task Handle(ShippingFailedEvent message)
    {
        _logger.LogInformation("Handling ShippingFailedEvent for OrderId: {OrderId}", message.OrderId);
        Data.ShippingFailed = true;
        TryComplete();
    }

    private void TryComplete()
    {
        if (Data.IsCompleted()) MarkAsComplete();
    }
}
