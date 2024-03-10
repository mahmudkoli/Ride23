using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

// Shipping Command Handlers
public class ShipOrderCommandHandler : IHandleMessages<ShipOrderCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<ShipOrderCommandHandler> _logger;

    public ShipOrderCommandHandler(IBus bus, ILogger<ShipOrderCommandHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(ShipOrderCommand message)
    {
        // Perform order shipping logic here

        // If order shipped successfully
        await _bus.Send(new OrderShippedEvent(message.OrderId));

        // If shipping failed
        // await _bus.Send(new ShippingFailedEvent(message.OrderId));
    }
}
