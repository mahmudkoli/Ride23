using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Orders.Sagas;

public class SendOrderConfirmationEmailHandler : IHandleMessages<SendOrderConfirmationEmail>
{
    private readonly ILogger<SendOrderConfirmationEmailHandler> _logger;

    public SendOrderConfirmationEmailHandler(ILogger<SendOrderConfirmationEmailHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SendOrderConfirmationEmail message)
    {
        _logger.LogInformation("Sending order confirmation {@OrderId}", message.OrderId);

        await Task.Delay(10000);

        _logger.LogInformation("Sending confirmation sent {@OrderId}", message.OrderId);
    }
}
