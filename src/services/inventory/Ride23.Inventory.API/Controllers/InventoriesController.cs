using Ride23.Inventory.Application.Inventories.Dtos;
using Ride23.Inventory.Application.Inventories.Features;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ride23.Inventory.Api.Controllers;

public class InventoriesController : BaseApiController
{
    private readonly ILogger<InventoriesController> _logger;

    public InventoriesController(ILogger<InventoriesController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = nameof(AddInventoryAsync))]
    [Authorize("inventory:write")]
    [ProducesResponseType(201, Type = typeof(InventoryDto))]
    public async Task<IActionResult> AddInventoryAsync(AddInventoryDto request)
    {
        var command = new AddInventory.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute(nameof(GetInventoryAsync), new { commandResponse.Id }, commandResponse);
    }

    [HttpGet("{id:guid}", Name = nameof(GetInventoryAsync))]
    [Authorize("inventory:read")]
    [ProducesResponseType(200, Type = typeof(InventoryDetailsDto))]
    public async Task<IActionResult> GetInventoryAsync(Guid id)
    {
        var query = new GetInventoryDetails.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpGet(Name = nameof(GetInventoriesAsync))]
    [Authorize("inventory:read")]
    [ProducesResponseType(200, Type = typeof(PagedList<InventoryDto>))]
    public async Task<IActionResult> GetInventoriesAsync([FromQuery] InventoriesParametersDto parameters)
    {
        var query = new GetInventories.Query(parameters);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpPut("{id:guid}", Name = nameof(UpdateInventoriesAsync))]
    [Authorize("inventory:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateInventoriesAsync(Guid id, UpdateInventoryDto updateInventoryDto)
    {
        var command = new UpdateInventory.Command(updateInventoryDto, id);
        await Mediator.Send(command);

        return NoContent();
    }
}
