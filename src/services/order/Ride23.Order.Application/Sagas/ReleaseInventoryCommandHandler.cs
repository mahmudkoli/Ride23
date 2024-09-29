using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class ReleaseInventoryCommandHandler : IHandleMessages<ReleaseInventoryCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<ReleaseInventoryCommandHandler> _logger;

    public ReleaseInventoryCommandHandler(IBus bus, ILogger<ReleaseInventoryCommandHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(ReleaseInventoryCommand message)
    {
        // If release successful
        await _bus.Send(new InventoryReleasedEvent(message.OrderId));

        // If release failed
        // await _bus.Send(new InventoryReleaseFailedEvent(message.OrderId));
    }
}
