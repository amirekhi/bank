using MyApp.Application.Abstractions;
using MyApp.Domain.BankAccounts;
using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.BankAccounts.Commands.OpenAccount;

public class OpenAccountHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public OpenAccountHandler(
        IAccountRepository accountRepository,
        IUserRepository userRepository,
        ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task Handle(OpenAccountCommand command, CancellationToken ct)
    {
        var ownerId = _currentUser.UserId;
        var owner = await _userRepository.GetById(ownerId, ct)
            ?? throw new UserNotFoundException(ownerId);

        owner.EnsureActive();

        var account = new Account(command.AccountNumber, owner.Id, command.Currency);
        await _accountRepository.Save(account, ct);
    }
}
