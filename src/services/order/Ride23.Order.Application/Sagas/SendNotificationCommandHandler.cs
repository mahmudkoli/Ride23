using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class SendNotificationCommandHandler : IHandleMessages<SendNotificationCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<SendNotificationCommandHandler> _logger;

    public SendNotificationCommandHandler(IBus bus, ILogger<SendNotificationCommandHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(SendNotificationCommand message)
    {
        // If notification sent successfully
        await _bus.Send(new NotificationSentEvent(message.OrderId));
    }
}
