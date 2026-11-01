using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyApp.Api.Security;
using MyApp.Application.Users.Commands.Login;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;

    public AuthController(LoginHandler loginHandler) => _loginHandler = loginHandler;

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting(Policies.AuthRateLimit)]
    public async Task<ActionResult<LoginResult>> Login(LoginCommand command, CancellationToken ct)
        => Ok(await _loginHandler.Handle(command, ct));
}
