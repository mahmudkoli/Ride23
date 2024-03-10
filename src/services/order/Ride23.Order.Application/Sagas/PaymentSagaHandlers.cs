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

        // Handle payment processed
        Data.PaymentProcessed = true; // Update the PaymentProcessed flag

        // No need to send the next event since payment processing is completed
    }

    public async Task Handle(PaymentFailedEvent message)
    {
        _logger.LogInformation("Handling PaymentFailedEvent for OrderId: {OrderId}", message.OrderId);

        // Handle payment failure
        Data.PaymentFailed = true; // Update the PaymentFailed flag
        //MarkAsComplete(); // Mark the saga as complete for payment failure

        // No need to send the next event since the saga is completed
    }

    public async Task Handle(RefundProcessedEvent message)
    {
        _logger.LogInformation("Handling RefundProcessedEvent for OrderId: {OrderId}", message.OrderId);

        // Handle refund processed
        Data.Refunded = true; // Update the Refunded flag

        // No need to send the next event since refund processing is completed
    }

    public async Task Handle(RefundFailedEvent message)
    {
        _logger.LogInformation("Handling RefundFailedEvent for OrderId: {OrderId}", message.OrderId);

        // Handle refund failure
        Data.RefundFailed = true; // Update the RefundFailed flag
        //MarkAsComplete(); // Mark the saga as complete for refund failure

        // No need to send the next event since the saga is completed
    }
}
