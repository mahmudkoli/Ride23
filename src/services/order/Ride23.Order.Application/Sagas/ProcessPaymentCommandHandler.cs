using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class ProcessPaymentCommandHandler : IHandleMessages<ProcessPaymentCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;

    public ProcessPaymentCommandHandler(IBus bus, ILogger<ProcessPaymentCommandHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(ProcessPaymentCommand message)
    {
        // If payment processed successfully
        await _bus.Send(new PaymentProcessedEvent(message.OrderId));

        // If payment failed
        // await _bus.Send(new PaymentFailedEvent(message.OrderId));
    }
}
