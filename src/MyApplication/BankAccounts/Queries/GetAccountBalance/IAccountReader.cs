// MyApp.Application/BankAccounts/Queries/GetAccountBalance/IAccountReader.cs
namespace MyApp.Application.BankAccounts.Queries.GetAccountBalance;

public interface IAccountReader
{
    Task<AccountBalanceDto?> GetBalance(string accountNumber, CancellationToken ct);
}