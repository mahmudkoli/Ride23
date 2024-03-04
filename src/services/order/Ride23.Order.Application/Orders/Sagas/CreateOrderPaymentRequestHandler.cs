using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Orders.Sagas;

public class CreateOrderPaymentRequestHandler : IHandleMessages<SendOrderConfirmationEmail>
{
    private readonly ILogger<CreateOrderPaymentRequestHandler> _logger;
    private readonly IBus _bus;

    public CreateOrderPaymentRequestHandler(ILogger<CreateOrderPaymentRequestHandler> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task Handle(SendOrderConfirmationEmail message)
    {
        _logger.LogInformation("Starting payment request {@OrderId}", message.OrderId);

        await Task.Delay(10000);

        _logger.LogInformation("Payment request started {@OrderId}", message.OrderId);

        await _bus.Send(new OrderPaymentRequestSent(message.OrderId));
    }
}