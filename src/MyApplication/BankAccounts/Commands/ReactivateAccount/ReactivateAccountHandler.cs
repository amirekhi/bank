using MyApp.Domain.BankAccounts;
using MyApp.Domain.BankAccounts.Exceptions;

namespace MyApp.Application.BankAccounts.Commands.ReactivateAccount;

public record ReactivateAccountCommand(string AccountNumber);

public class ReactivateAccountHandler
{
    private readonly IAccountRepository _accountRepository;

    public ReactivateAccountHandler(IAccountRepository accountRepository) => _accountRepository = accountRepository;

    public async Task Handle(ReactivateAccountCommand command, CancellationToken ct)
    {
        var account = await _accountRepository.GetByAccountNumber(command.AccountNumber, ct)
            ?? throw new AccountNotFoundException(command.AccountNumber);

        account.Reactivate();
        await _accountRepository.Save(account, ct);
    }
}
