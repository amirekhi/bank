using MyApp.Domain.BankAccounts;
using MyApp.Domain.BankAccounts.Exceptions;

namespace MyApp.Application.BankAccounts.Commands.SuspendAccount;

public record SuspendAccountCommand(string AccountNumber);

public class SuspendAccountHandler
{
    private readonly IAccountRepository _accountRepository;

    public SuspendAccountHandler(IAccountRepository accountRepository) => _accountRepository = accountRepository;

    public async Task Handle(SuspendAccountCommand command, CancellationToken ct)
    {
        var account = await _accountRepository.GetByAccountNumber(command.AccountNumber, ct)
            ?? throw new AccountNotFoundException(command.AccountNumber);

        account.Suspend();
        await _accountRepository.Save(account, ct);
    }
}
