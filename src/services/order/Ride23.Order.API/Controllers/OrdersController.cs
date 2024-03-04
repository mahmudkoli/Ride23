using Ride23.Order.Application.Orders.Dtos;
using Ride23.Order.Application.Orders.Features;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ride23.Order.Api.Controllers;

public class OrdersController : BaseApiController
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = nameof(AddOrderAsync))]
    [Authorize("order:write")]
    [ProducesResponseType(201, Type = typeof(OrderDto))]
    public async Task<IActionResult> AddOrderAsync(AddOrderDto request)
    {
        var command = new AddOrder.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute(nameof(GetOrderAsync), new { commandResponse.Id }, commandResponse);
    }

    [HttpGet("{id:guid}", Name = nameof(GetOrderAsync))]
    [Authorize("order:read")]
    [ProducesResponseType(200, Type = typeof(OrderDetailsDto))]
    public async Task<IActionResult> GetOrderAsync(Guid id)
    {
        var query = new GetOrderDetails.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpGet(Name = nameof(GetOrdersAsync))]
    [Authorize("order:read")]
    [ProducesResponseType(200, Type = typeof(PagedList<OrderDto>))]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] OrdersParametersDto parameters)
    {
        var query = new GetOrders.Query(parameters);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpPut("{id:guid}", Name = nameof(UpdateOrdersAsync))]
    [Authorize("order:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateOrdersAsync(Guid id, UpdateOrderDto updateOrderDto)
    {
        var command = new UpdateOrder.Command(updateOrderDto, id);
        await Mediator.Send(command);

        return NoContent();
    }
}
