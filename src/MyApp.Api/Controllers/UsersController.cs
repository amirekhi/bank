using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Users.Commands.RegisterUser;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly RegisterUserHandler _registerHandler;

    public UsersController(RegisterUserHandler registerHandler)
    {
        _registerHandler = registerHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken ct)
    {
        var userId = await _registerHandler.Handle(command, ct);
        return Ok(new { userId });
    }
}
