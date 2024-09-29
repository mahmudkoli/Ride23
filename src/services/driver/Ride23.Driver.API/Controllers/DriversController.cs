using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ride23.Driver.Application.Drivers.Dtos;
using Ride23.Driver.Application.Drivers.Features;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Infrastructure.Controllers;

namespace Ride23.Driver.Api.Controllers;

public class DriversController : BaseApiController
{
    private readonly ILogger<DriversController> _logger;

    public DriversController(ILogger<DriversController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = nameof(AddDriverAsync))]
    [Authorize("driver:write")]
    [ProducesResponseType(201, Type = typeof(DriverDto))]
    public async Task<IActionResult> AddDriverAsync([FromBody] AddDriverDto request)
    {
        var command = new AddDriver.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute(nameof(GetDriverAsync), new { commandResponse.Id }, commandResponse);
    }

    [HttpGet("{id:guid}", Name = nameof(GetDriverAsync))]
    [Authorize("driver:read")]
    [ProducesResponseType(200, Type = typeof(DriverDetailsDto))]
    public async Task<IActionResult> GetDriverAsync(Guid id)
    {
        var query = new GetDriverDetails.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpGet(Name = nameof(GetDriversAsync))]
    [Authorize("driver:read")]
    [ProducesResponseType(200, Type = typeof(PagedList<DriverDto>))]
    public async Task<IActionResult> GetDriversAsync([FromQuery] DriversParametersDto parameters)
    {
        var query = new GetDrivers.Query(parameters);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpDelete("{id:guid}", Name = nameof(DeleteDriversAsync))]
    [Authorize("driver:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteDriversAsync(Guid id)
    {
        var command = new DeleteDriver.Command(id);
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id:guid}", Name = nameof(UpdateDriversAsync))]
    [Authorize("driver:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateDriversAsync(Guid id, UpdateDriverDto updateDriverDto)
    {
        var command = new UpdateDriver.Command(updateDriverDto, id);
        await Mediator.Send(command);

        return NoContent();
    }
}
