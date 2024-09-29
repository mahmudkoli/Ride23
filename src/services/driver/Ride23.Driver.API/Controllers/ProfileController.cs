using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ride23.Driver.Application.Drivers.Dtos;
using Ride23.Driver.Application.Drivers.Features;
using Ride23.Framework.Infrastructure.Controllers;

namespace Ride23.Driver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseApiController
    {

        [HttpPost(Name = nameof(RegisterDriverAsync))]
        [AllowAnonymous]
        [ProducesResponseType(201, Type = typeof(DriverDto))]
        public async Task<IActionResult> RegisterDriverAsync([FromBody] RegisterDriverDto request)
        {
            var command = new RegisterDriver.Command(request);
            var commandResponse = await Mediator.Send(command);

            return CreatedAtRoute(nameof(GetRegisterDriverAsync), new { commandResponse.Id }, commandResponse);
        }

        [HttpGet("{id:guid}", Name = nameof(GetRegisterDriverAsync))]
        [Authorize("driver:read")]
        [ProducesResponseType(200, Type = typeof(DriverDetailsDto))]
        public async Task<IActionResult> GetRegisterDriverAsync(Guid id)
        {
            var query = new GetDriverDetails.Query(id);
            var queryResponse = await Mediator.Send(query);

            return Ok(queryResponse);
        }
    }
}
