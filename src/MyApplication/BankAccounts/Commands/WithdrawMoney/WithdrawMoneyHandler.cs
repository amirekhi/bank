// MyApp.Application/BankAccounts/Commands/WithdrawMoney/WithdrawMoneyHandler.cs
using MyApp.Domain.BankAccounts;
using MyApp.Domain.BankAccounts.Exceptions;

namespace MyApplication.BankAccounts.Commands.WithdrawMoney;

public class WithdrawMoneyHandler
{
    private readonly IAccountRepository _accountRepository;

    public WithdrawMoneyHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task Handle(WithdrawMoneyCommand command, CancellationToken ct)
    {
        var account = await _accountRepository.GetByAccountNumber(command.AccountNumber, ct)
            ?? throw new AccountNotFoundException(command.AccountNumber);

        var amount = new Money(command.Amount, command.Currency);
        account.Withdraw(amount, command.Description);

        await _accountRepository.Save(account, ct);
    }
}