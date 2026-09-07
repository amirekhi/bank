// MyApp.Api/Controllers/AccountsController.cs
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;
using MyApp.Domain.BankAccounts.Exceptions;
using MyApplication.BankAccounts.Commands.DepositMoney;
using MyApplication.BankAccounts.Commands.WithdrawMoney;
using MyApplication.BankAccounts.Commands.OpenAccount;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly OpenAccountHandler _openHandler;
    private readonly DepositMoneyHandler _depositHandler;
    private readonly WithdrawMoneyHandler _withdrawHandler;
    private readonly GetAccountBalanceHandler _balanceHandler;

    public AccountsController(
        OpenAccountHandler openHandler,
        DepositMoneyHandler depositHandler,
        WithdrawMoneyHandler withdrawHandler,
        GetAccountBalanceHandler balanceHandler)
    {
        _openHandler = openHandler;
        _depositHandler = depositHandler;
        _withdrawHandler = withdrawHandler;
        _balanceHandler = balanceHandler;
    }

    [HttpPost("open")]
    public async Task<IActionResult> Open(OpenAccountCommand command, CancellationToken ct)
    {
        await _openHandler.Handle(command, ct);
        return Ok();
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(DepositMoneyCommand command, CancellationToken ct)
    {
        try
        {
            await _depositHandler.Handle(command, ct);
            return Ok();
        }
        catch (AccountNotFoundException ex) { return NotFound(ex.Message); }
        catch (AccountClosedException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(WithdrawMoneyCommand command, CancellationToken ct)
    {
        try
        {
            await _withdrawHandler.Handle(command, ct);
            return Ok();
        }
        catch (AccountNotFoundException ex) { return NotFound(ex.Message); }
        catch (InsufficientFundsException ex) { return BadRequest(ex.Message); }
        catch (AccountClosedException ex) { return BadRequest(ex.Message); }
    }

    [HttpGet("{accountNumber}/balance")]
    public async Task<IActionResult> GetBalance(string accountNumber, CancellationToken ct)
    {
        var result = await _balanceHandler.Handle(new GetAccountBalanceQuery(accountNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }
}