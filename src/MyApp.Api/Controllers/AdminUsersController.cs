using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Api.Security;
using MyApp.Application.Users.Commands.ActivateUser;
using MyApp.Application.Users.Commands.ReinstateUser;
using MyApp.Application.Users.Commands.SuspendUser;
using MyApp.Application.Users.Queries.ListUsers;
using MyApp.Domain.Users;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Policy = Policies.AdminOnly)]
public class AdminUsersController : ControllerBase
{
    private readonly ListUsersHandler _listHandler;
    private readonly ActivateUserHandler _activateHandler;
    private readonly SuspendUserHandler _suspendHandler;
    private readonly ReinstateUserHandler _reinstateHandler;

    public AdminUsersController(
        ListUsersHandler listHandler,
        ActivateUserHandler activateHandler,
        SuspendUserHandler suspendHandler,
        ReinstateUserHandler reinstateHandler)
    {
        _listHandler = listHandler;
        _activateHandler = activateHandler;
        _suspendHandler = suspendHandler;
        _reinstateHandler = reinstateHandler;
    }

    /// <summary>Lists users, optionally filtered by status (e.g. PendingActivation).</summary>
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] UserStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _listHandler.Handle(new ListUsersQuery(status, page, pageSize), ct));

    /// <summary>PendingActivation -> Active. (Later: identity verification such as SSN checks happens before this.)</summary>
    [HttpPost("{userId:guid}/activate")]
    public async Task<IActionResult> Activate(Guid userId, CancellationToken ct)
    {
        await _activateHandler.Handle(new ActivateUserCommand(userId), ct);
        return NoContent();
    }

    /// <summary>Active -> Suspended.</summary>
    [HttpPost("{userId:guid}/suspend")]
    public async Task<IActionResult> Suspend(Guid userId, CancellationToken ct)
    {
        await _suspendHandler.Handle(new SuspendUserCommand(userId), ct);
        return NoContent();
    }

    /// <summary>Suspended -> Active.</summary>
    [HttpPost("{userId:guid}/reinstate")]
    public async Task<IActionResult> Reinstate(Guid userId, CancellationToken ct)
    {
        await _reinstateHandler.Handle(new ReinstateUserCommand(userId), ct);
        return NoContent();
    }
}
