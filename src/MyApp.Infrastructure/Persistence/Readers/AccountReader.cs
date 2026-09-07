// MyApp.Infrastructure/Persistence/Readers/AccountReader.cs
using Microsoft.EntityFrameworkCore;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;

namespace MyApp.Infrastructure.Persistence.Readers;

public class AccountReader : IAccountReader
{
    private readonly AppDbContext _context;

    public AccountReader(AppDbContext context) => _context = context;

    public async Task<AccountBalanceDto?> GetBalance(string accountNumber, CancellationToken ct)
    {
        return await _context.Accounts
            .Where(a => a.AccountNumber == accountNumber)
            .Select(a => new AccountBalanceDto(
                a.AccountNumber,
                a.Balance.Amount,
                a.Balance.Currency,
                a.Status.ToString()))
            .FirstOrDefaultAsync(ct);
    }
}