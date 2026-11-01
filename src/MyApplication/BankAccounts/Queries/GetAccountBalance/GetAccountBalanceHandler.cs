using MyApp.Application.Abstractions;

namespace MyApp.Application.BankAccounts.Queries.GetAccountBalance;

public class GetAccountBalanceHandler
{
    private readonly IAccountReader _reader;
    private readonly ICurrentUser _currentUser;

    public GetAccountBalanceHandler(IAccountReader reader, ICurrentUser currentUser)
    {
        _reader = reader;
        _currentUser = currentUser;
    }

    public Task<AccountBalanceDto?> Handle(GetAccountBalanceQuery query, CancellationToken ct)
    {
        // Admins may read any account; customers only their own (anything else looks like "not found").
        Guid? restrictToOwner = _currentUser.IsAdmin ? null : _currentUser.UserId;
        return _reader.GetBalance(query.AccountNumber, restrictToOwner, ct);
    }
}
