using MyApp.Application.Abstractions;
using MyApp.Domain.BankAccounts;
using MyApp.Domain.BankAccounts.Exceptions;
using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.BankAccounts.Commands.DepositMoney;

public class DepositMoneyHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public DepositMoneyHandler(
        IAccountRepository accountRepository,
        IUserRepository userRepository,
        ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task Handle(DepositMoneyCommand command, CancellationToken ct)
    {
        var callerId = _currentUser.UserId;

        var account = await _accountRepository.GetByAccountNumber(command.AccountNumber, ct)
            ?? throw new AccountNotFoundException(command.AccountNumber);

        // Someone else's account looks exactly like a missing one.
        if (!account.IsOwnedBy(callerId))
            throw new AccountNotFoundException(command.AccountNumber);

        var caller = await _userRepository.GetById(callerId, ct)
            ?? throw new UserNotFoundException(callerId);
        caller.EnsureActive();

        var amount = new Money(command.Amount, command.Currency);
        account.Deposit(amount, command.Description);

        await _accountRepository.Save(account, ct);
    }
}
