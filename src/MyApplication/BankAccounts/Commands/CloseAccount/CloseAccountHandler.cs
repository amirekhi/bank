using MyApp.Domain.BankAccounts;
using MyApp.Domain.BankAccounts.Exceptions;

namespace MyApp.Application.BankAccounts.Commands.CloseAccount;

public record CloseAccountCommand(string AccountNumber);

public class CloseAccountHandler
{
    private readonly IAccountRepository _accountRepository;

    public CloseAccountHandler(IAccountRepository accountRepository) => _accountRepository = accountRepository;

    public async Task Handle(CloseAccountCommand command, CancellationToken ct)
    {
        var account = await _accountRepository.GetByAccountNumber(command.AccountNumber, ct)
            ?? throw new AccountNotFoundException(command.AccountNumber);

        account.Close();
        await _accountRepository.Save(account, ct);
    }
}
