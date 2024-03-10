using Rebus.Sagas;

namespace Ride23.Saga.Order;

// Marker interfaces
public interface IOrderMap;
public interface IInventoryMap;

public class OrderProcessingSagaData : ISagaData
{
    public Guid Id { get; set; }
    public int Revision { get; set; }
    public Guid OrderId { get; set; }
    public bool InventoryReserved { get; set; }
    public bool InventoryReservationFailed { get; set; }
    public bool InventoryReleased { get; set; }
    public bool InventoryReleaseFailed { get; set; }
    public bool PaymentProcessed { get; set; }
    public bool PaymentFailed { get; set; }
    public bool NotificationSent { get; set; }
    public bool Shipped { get; set; }
    public bool ShippingFailed { get; set; }
    public bool Cancelled { get; set; }
    public bool CancellationFailed { get; set; }
    public bool Refunded { get; set; }
    public bool RefundFailed { get; set; }
    public bool Success { get; set; }
    public bool Completed { get; set; }
}

public record ReserveInventoryCommand(Guid OrderId) : IInventoryMap;
public record ReleaseInventoryCommand(Guid OrderId) : IInventoryMap;
public record ProcessPaymentCommand(Guid OrderId) : IOrderMap;
public record RefundPaymentCommand(Guid OrderId) : IOrderMap;
public record SendNotificationCommand(Guid OrderId) : IOrderMap;
public record ShipOrderCommand(Guid OrderId) : IOrderMap;

public record OrderCreatedEvent(Guid OrderId) : IOrderMap;
public record OrderCancelledEvent(Guid OrderId) : IOrderMap;
public record OrderProcessingSuccessEvent(Guid OrderId) : IOrderMap;
public record OrderProcessingCompleteEvent(Guid OrderId) : IOrderMap;
public record InventoryReservedEvent(Guid OrderId) : IInventoryMap;
public record InventoryReservationFailedEvent(Guid OrderId) : IInventoryMap;
public record InventoryReleasedEvent(Guid OrderId) : IInventoryMap;
public record InventoryReleaseFailedEvent(Guid OrderId) : IInventoryMap;
public record PaymentProcessedEvent(Guid OrderId) : IOrderMap;
public record PaymentFailedEvent(Guid OrderId) : IOrderMap;
public record RefundProcessedEvent(Guid OrderId) : IOrderMap;
public record RefundFailedEvent(Guid OrderId) : IOrderMap;
public record NotificationSentEvent(Guid OrderId) : IOrderMap;
public record OrderShippedEvent(Guid OrderId) : IOrderMap;
public record ShippingFailedEvent(Guid OrderId) : IOrderMap;
