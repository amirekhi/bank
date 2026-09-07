// MyApp.Application/BankAccounts/Commands/DepositMoney/DepositMoneyHandler.cs
using MyApp.Domain.BankAccounts;
using MyApp.Domain.BankAccounts.Exceptions;
using MyApplication.BankAccounts.Commands.DepositMoney;

namespace MyApplication.BankAccounts.Commands.DepositMoney;

public class DepositMoneyHandler
{
    private readonly IAccountRepository _accountRepository;

    public DepositMoneyHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task Handle(DepositMoneyCommand command, CancellationToken ct)
    {
        var account = await _accountRepository.GetByAccountNumber(command.AccountNumber, ct)
            ?? throw new AccountNotFoundException(command.AccountNumber);

        var amount = new Money(command.Amount, command.Currency);
        account.Deposit(amount, command.Description);

        await _accountRepository.Save(account, ct);
    }
}