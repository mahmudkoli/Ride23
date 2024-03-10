using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Inventory.Application.Sagas;

// Inventory Command Handlers
public class ReserveInventoryCommandHandler : IHandleMessages<ReserveInventoryCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<ReserveInventoryCommandHandler> _logger;

    public ReserveInventoryCommandHandler(IBus bus, ILogger<ReserveInventoryCommandHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(ReserveInventoryCommand message)
    {
        // Perform inventory reservation logic here

        // If reservation successful
        await _bus.Send(new InventoryReservedEvent(message.OrderId));

        // If reservation failed
        // await _bus.Send(new InventoryReservationFailedEvent(message.OrderId));
    }
}
