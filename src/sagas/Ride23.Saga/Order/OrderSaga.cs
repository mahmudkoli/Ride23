using Rebus.Sagas;

namespace Ride23.Saga.Order;

public class OrderCreateSagaData : ISagaData
{
    public Guid Id { get; set; }
    public int Revision { get; set; }
    public Guid OrderId { get; set; }
    public bool ConfirmationEmailSent { get; set; }
    public bool PaymentRequestSent { get; set; }
}

public record OrderCreatedEvent(Guid OrderId);
public record OrderConfirmationEmailSent(Guid OrderId);
public record OrderPaymentRequestSent(Guid OrderId);
public record SendOrderConfirmationEmail(Guid OrderId);
public record CreateOrderPaymentRequest(Guid OrderId);
