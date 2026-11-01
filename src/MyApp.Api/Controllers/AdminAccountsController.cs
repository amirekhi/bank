using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Api.Security;
using MyApp.Application.BankAccounts.Commands.CloseAccount;
using MyApp.Application.BankAccounts.Commands.ReactivateAccount;
using MyApp.Application.BankAccounts.Commands.SuspendAccount;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/admin/accounts")]
[Authorize(Policy = Policies.AdminOnly)]
public class AdminAccountsController : ControllerBase
{
    private readonly SuspendAccountHandler _suspendHandler;
    private readonly ReactivateAccountHandler _reactivateHandler;
    private readonly CloseAccountHandler _closeHandler;

    public AdminAccountsController(
        SuspendAccountHandler suspendHandler,
        ReactivateAccountHandler reactivateHandler,
        CloseAccountHandler closeHandler)
    {
        _suspendHandler = suspendHandler;
        _reactivateHandler = reactivateHandler;
        _closeHandler = closeHandler;
    }

    [HttpPost("{accountNumber}/suspend")]
    public async Task<IActionResult> Suspend(string accountNumber, CancellationToken ct)
    {
        await _suspendHandler.Handle(new SuspendAccountCommand(accountNumber), ct);
        return NoContent();
    }

    [HttpPost("{accountNumber}/reactivate")]
    public async Task<IActionResult> Reactivate(string accountNumber, CancellationToken ct)
    {
        await _reactivateHandler.Handle(new ReactivateAccountCommand(accountNumber), ct);
        return NoContent();
    }

    [HttpPost("{accountNumber}/close")]
    public async Task<IActionResult> Close(string accountNumber, CancellationToken ct)
    {
        await _closeHandler.Handle(new CloseAccountCommand(accountNumber), ct);
        return NoContent();
    }
}
