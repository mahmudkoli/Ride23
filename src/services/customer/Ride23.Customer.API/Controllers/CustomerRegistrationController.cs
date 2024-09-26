using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ride23.Customer.Api.Controllers;
using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Customer.Application.Customers.Features;
using Ride23.Framework.Infrastructure.Controllers;

namespace Ride23.Customer.API.Controllers
{
    public class CustomerRegistrationController : BaseApiController
    {
        private readonly ILogger<CustomersController> _logger;

        public CustomerRegistrationController(
            ILogger<CustomersController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = nameof(RegisterCustomerAsync))]
        [AllowAnonymous]
        [ProducesResponseType(201, Type = typeof(CustomerDto))]
        public async Task<IActionResult> RegisterCustomerAsync([FromBody] RegisterCustomerDto request)
        {
            var command = new RegisterCustomer.Command(request);
            var commandResponse = await Mediator.Send(command);

            return Ok(commandResponse);
        }
    }
}
