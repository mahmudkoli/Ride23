using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Customer.Application.Customers.Features;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ride23.Customer.Api.Controllers;

public class CustomersController : BaseApiController
{
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ILogger<CustomersController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = nameof(AddCustomerAsync))]
    [Authorize("customer:write")]
    [ProducesResponseType(201, Type = typeof(CustomerDto))]
    public async Task<IActionResult> AddCustomerAsync(AddCustomerDto request)
    {
        var command = new AddCustomer.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute(nameof(GetCustomerAsync), new { commandResponse.Id }, commandResponse);
    }

    [HttpGet("{id:guid}", Name = nameof(GetCustomerAsync))]
    [Authorize("customer:read")]
    [ProducesResponseType(200, Type = typeof(CustomerDetailsDto))]
    public async Task<IActionResult> GetCustomerAsync(Guid id)
    {
        var query = new GetCustomerDetails.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpGet(Name = nameof(GetCustomersAsync))]
    [Authorize("customer:read")]
    [ProducesResponseType(200, Type = typeof(PagedList<CustomerDto>))]
    public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomersParametersDto parameters)
    {
        var query = new GetCustomers.Query(parameters);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpDelete("{id:guid}", Name = nameof(DeleteCustomersAsync))]
    [Authorize("customer:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteCustomersAsync(Guid id)
    {
        var command = new DeleteCustomer.Command(id);
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id:guid}", Name = nameof(UpdateCustomersAsync))]
    [Authorize("customer:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateCustomersAsync(Guid id, UpdateCustomerDto updateCustomerDto)
    {
        var command = new UpdateCustomer.Command(updateCustomerDto, id);
        await Mediator.Send(command);

        return NoContent();
    }
}
