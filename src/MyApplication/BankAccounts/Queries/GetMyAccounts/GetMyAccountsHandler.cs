using MyApp.Application.Abstractions;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;

namespace MyApp.Application.BankAccounts.Queries.GetMyAccounts;

public record GetMyAccountsQuery;

public class GetMyAccountsHandler
{
    private readonly IAccountReader _reader;
    private readonly ICurrentUser _currentUser;

    public GetMyAccountsHandler(IAccountReader reader, ICurrentUser currentUser)
    {
        _reader = reader;
        _currentUser = currentUser;
    }

    public Task<IReadOnlyList<AccountBalanceDto>> Handle(GetMyAccountsQuery query, CancellationToken ct)
        => _reader.ListByOwner(_currentUser.UserId, ct);
}
