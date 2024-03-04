using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Orders.Sagas;

public class OrderCreateSaga : Saga<OrderCreateSagaData>,
    IAmInitiatedBy<OrderCreatedEvent>,
    IHandleMessages<OrderConfirmationEmailSent>
{
    private readonly IBus _bus;

    public OrderCreateSaga(IBus bus)
    {
        _bus = bus;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderCreateSagaData> config)
    {
        config.Correlate<OrderCreatedEvent>(m => m.OrderId, s => s.OrderId);
        config.Correlate<OrderConfirmationEmailSent>(m => m.OrderId, s => s.OrderId);
    }

    public async Task Handle(OrderCreatedEvent message)
    {
        if(!IsNew) return;

        await _bus.Send(new SendOrderConfirmationEmail(message.OrderId));
    }

    public async Task Handle(OrderConfirmationEmailSent message)
    {
        Data.ConfirmationEmailSent = true;

        await _bus.Send(new CreateOrderPaymentRequest(message.OrderId));
    }
}

public class OrderPaymentSaga : Saga<OrderCreateSagaData>,
    IHandleMessages<OrderPaymentRequestSent>
{
    private readonly IBus _bus;

    public OrderPaymentSaga(IBus bus)
    {
        _bus = bus;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderCreateSagaData> config)
    {
        config.Correlate<OrderPaymentRequestSent>(m => m.OrderId, s => s.OrderId);
    }

    public async Task Handle(OrderPaymentRequestSent message)
    {
        await Task.Delay(10000);

        Data.PaymentRequestSent = true;

        MarkAsComplete();

        Console.WriteLine("Order payment has been successfully done.");

        //return Task.CompletedTask;
    }
}
