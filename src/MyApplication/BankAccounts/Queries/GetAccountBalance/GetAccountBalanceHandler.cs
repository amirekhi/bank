// MyApp.Application/BankAccounts/Queries/GetAccountBalance/GetAccountBalanceHandler.cs
namespace MyApp.Application.BankAccounts.Queries.GetAccountBalance;

public class GetAccountBalanceHandler
{
    private readonly IAccountReader _reader;

    public GetAccountBalanceHandler(IAccountReader reader) => _reader = reader;

    public Task<AccountBalanceDto?> Handle(GetAccountBalanceQuery query, CancellationToken ct)
        => _reader.GetBalance(query.AccountNumber, ct);
}