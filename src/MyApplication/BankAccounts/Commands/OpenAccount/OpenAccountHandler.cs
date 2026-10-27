using MyApp.Domain.BankAccounts;
using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.BankAccounts.Commands.OpenAccount;

public class OpenAccountHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;

    public OpenAccountHandler(IAccountRepository accountRepository, IUserRepository userRepository)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(OpenAccountCommand command, CancellationToken ct)
    {
        // The owner must exist before an account can be opened for them.
        _ = await _userRepository.GetById(command.OwnerId, ct)
            ?? throw new UserNotFoundException(command.OwnerId);

        var overdraftLimit = command.OverdraftLimit > 0
            ? new Money(command.OverdraftLimit, command.Currency)
            : null;

        var account = new Account(command.AccountNumber, command.OwnerId, command.Currency, overdraftLimit);
        await _accountRepository.Save(account, ct);
    }
}
