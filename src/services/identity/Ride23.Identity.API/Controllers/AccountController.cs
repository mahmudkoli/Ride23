using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ride23.Identity.Domain.Users;

namespace Ride23.Identity.Api.Controllers;

[Authorize]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(
        UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("~/account/register"), IgnoreAntiforgeryToken, Produces("application/json")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto register)
    {
        var user = await _userManager.FindByNameAsync(register.Email);
        if (user != null)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }

        user = new AppUser { UserName = register.Email, Email = register.Email };
        var result = await _userManager.CreateAsync(user, register.Password);
        if (result.Succeeded)
        {
            return Ok($"New user {user.UserName} registered successfully");
        }

        return BadRequest($"{string.Join(", ", result.Errors.Select(e => e.Description))}");
    }
}

public class RegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
