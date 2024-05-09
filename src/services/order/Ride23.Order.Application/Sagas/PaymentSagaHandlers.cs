using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class PaymentSagaHandlers :
        Saga<OrderProcessingSagaData>,
        IHandleMessages<PaymentProcessedEvent>,
        IHandleMessages<PaymentFailedEvent>,
        IHandleMessages<RefundProcessedEvent>,
        IHandleMessages<RefundFailedEvent>
{
    private readonly IBus _bus;
    private readonly ILogger<PaymentSagaHandlers> _logger;

    public PaymentSagaHandlers(IBus bus, ILogger<PaymentSagaHandlers> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderProcessingSagaData> config)
    {
        config.Correlate<PaymentProcessedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<PaymentFailedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<RefundProcessedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<RefundFailedEvent>(msg => msg.OrderId, data => data.OrderId);
    }

    public async Task Handle(PaymentProcessedEvent message)
    {
        _logger.LogInformation("Handling PaymentProcessedEvent for OrderId: {OrderId}", message.OrderId);
        Data.PaymentProcessed = true;
    }

    public async Task Handle(PaymentFailedEvent message)
    {
        _logger.LogInformation("Handling PaymentFailedEvent for OrderId: {OrderId}", message.OrderId);
        Data.PaymentFailed = true;
        TryComplete();
    }

    public async Task Handle(RefundProcessedEvent message)
    {
        _logger.LogInformation("Handling RefundProcessedEvent for OrderId: {OrderId}", message.OrderId);
        Data.Refunded = true;
    }

    public async Task Handle(RefundFailedEvent message)
    {
        _logger.LogInformation("Handling RefundFailedEvent for OrderId: {OrderId}", message.OrderId);
        Data.RefundFailed = true;
        TryComplete();
    }

    private void TryComplete()
    {
        if (Data.IsCompleted()) MarkAsComplete();
    }
}
