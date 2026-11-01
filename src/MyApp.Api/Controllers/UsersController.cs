using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyApp.Api.Security;
using MyApp.Application.Users.Commands.RegisterUser;
using MyApp.Application.Users.Queries.GetCurrentUser;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly RegisterUserHandler _registerHandler;
    private readonly GetCurrentUserHandler _meHandler;

    public UsersController(RegisterUserHandler registerHandler, GetCurrentUserHandler meHandler)
    {
        _registerHandler = registerHandler;
        _meHandler = meHandler;
    }

    /// <summary>Creates a customer in PendingActivation status. An admin must activate it before login works.</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [EnableRateLimiting(Policies.AuthRateLimit)]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken ct)
    {
        var userId = await _registerHandler.Handle(command, ct);
        return Ok(new { userId });
    }

    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserDto>> Me(CancellationToken ct)
        => Ok(await _meHandler.Handle(ct));
}
