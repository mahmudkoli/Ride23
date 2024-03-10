using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Ride23.Saga.Order;

namespace Ride23.Inventory.Application.Sagas;

public class InventorySagaHandlers :
        Saga<OrderProcessingSagaData>,
        IHandleMessages<InventoryReservationFailedEvent>,
        IHandleMessages<InventoryReservedEvent>,
        IHandleMessages<OrderCancelledEvent>
{
    private readonly IBus _bus;
    private readonly ILogger<InventorySagaHandlers> _logger;

    public InventorySagaHandlers(IBus bus, ILogger<InventorySagaHandlers> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderProcessingSagaData> config)
    {
        config.Correlate<InventoryReservationFailedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<InventoryReservedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<OrderCancelledEvent>(msg => msg.OrderId, data => data.OrderId);
    }

    public async Task Handle(InventoryReservationFailedEvent message)
    {
        _logger.LogInformation("Handling InventoryReservationFailedEvent for OrderId: {OrderId}", message.OrderId);

        // Handle inventory reservation failure
        Data.InventoryReservationFailed = true; // Update the InventoryReservationFailed flag
        //MarkAsComplete(); // Mark the saga as complete for inventory reservation failure

        // No need to send the next event since the saga is completed
    }

    public async Task Handle(InventoryReservedEvent message)
    {
        _logger.LogInformation("Handling InventoryReservedEvent for OrderId: {OrderId}", message.OrderId);

        // Handle inventory reservation success
        Data.InventoryReserved = true; // Update the InventoryReserved flag

        // Send the next event
        await _bus.Send(new OrderProcessingSuccessEvent(message.OrderId));
    }

    public async Task Handle(OrderCancelledEvent message)
    {
        _logger.LogInformation("Handling OrderCancelledEvent for OrderId: {OrderId}", message.OrderId);

        // Handle order cancellation related to inventory
        Data.Cancelled = true; // Update the Cancelled flag
        //MarkAsComplete(); // Mark the saga as complete for order cancellation related to inventory

        // No need to send the next event since the saga is completed
    }
}
