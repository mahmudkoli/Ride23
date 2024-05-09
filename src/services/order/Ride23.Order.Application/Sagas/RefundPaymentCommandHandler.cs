using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class RefundPaymentCommandHandler : IHandleMessages<RefundPaymentCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<RefundPaymentCommandHandler> _logger;

    public RefundPaymentCommandHandler(IBus bus, ILogger<RefundPaymentCommandHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(RefundPaymentCommand message)
    {
        // If refund processed successfully
        await _bus.Send(new RefundProcessedEvent(message.OrderId));

        // If refund failed
        // await _bus.Send(new RefundFailedEvent(message.OrderId));
    }
}
