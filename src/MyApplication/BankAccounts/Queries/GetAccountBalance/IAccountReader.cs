namespace MyApp.Application.BankAccounts.Queries.GetAccountBalance;

public interface IAccountReader
{
    /// <param name="ownerId">When set, only an account owned by this user is returned.</param>
    Task<AccountBalanceDto?> GetBalance(string accountNumber, Guid? ownerId, CancellationToken ct);

    Task<IReadOnlyList<AccountBalanceDto>> ListByOwner(Guid ownerId, CancellationToken ct);
}
