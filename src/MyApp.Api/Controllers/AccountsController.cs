using Microsoft.AspNetCore.Mvc;
using MyApp.Application.BankAccounts.Commands.DepositMoney;
using MyApp.Application.BankAccounts.Commands.OpenAccount;
using MyApp.Application.BankAccounts.Commands.WithdrawMoney;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;
using MyApp.Application.BankAccounts.Queries.GetMyAccounts;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly OpenAccountHandler _openHandler;
    private readonly DepositMoneyHandler _depositHandler;
    private readonly WithdrawMoneyHandler _withdrawHandler;
    private readonly GetAccountBalanceHandler _balanceHandler;
    private readonly GetMyAccountsHandler _myAccountsHandler;

    public AccountsController(
        OpenAccountHandler openHandler,
        DepositMoneyHandler depositHandler,
        WithdrawMoneyHandler withdrawHandler,
        GetAccountBalanceHandler balanceHandler,
        GetMyAccountsHandler myAccountsHandler)
    {
        _openHandler = openHandler;
        _depositHandler = depositHandler;
        _withdrawHandler = withdrawHandler;
        _balanceHandler = balanceHandler;
        _myAccountsHandler = myAccountsHandler;
    }

    /// <summary>Opens an account owned by the authenticated user.</summary>
    [HttpPost("open")]
    public async Task<IActionResult> Open(OpenAccountCommand command, CancellationToken ct)
    {
        await _openHandler.Handle(command, ct);
        return CreatedAtAction(nameof(GetBalance), new { accountNumber = command.AccountNumber }, null);
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(DepositMoneyCommand command, CancellationToken ct)
    {
        await _depositHandler.Handle(command, ct);
        return Ok();
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(WithdrawMoneyCommand command, CancellationToken ct)
    {
        await _withdrawHandler.Handle(command, ct);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> MyAccounts(CancellationToken ct)
        => Ok(await _myAccountsHandler.Handle(new GetMyAccountsQuery(), ct));

    [HttpGet("{accountNumber}/balance")]
    public async Task<IActionResult> GetBalance(string accountNumber, CancellationToken ct)
    {
        var result = await _balanceHandler.Handle(new GetAccountBalanceQuery(accountNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }
}
