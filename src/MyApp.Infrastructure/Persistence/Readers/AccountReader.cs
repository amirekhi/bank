// MyApp.Infrastructure/Persistence/Readers/AccountReader.cs
using Microsoft.EntityFrameworkCore;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;

namespace MyApp.Infrastructure.Persistence.Readers;

public class AccountReader : IAccountReader
{
    private readonly AppDbContext _context;

    public AccountReader(AppDbContext context) => _context = context;

    public async Task<AccountBalanceDto?> GetBalance(string accountNumber, Guid? ownerId, CancellationToken ct)
    {
        var query = _context.Accounts
            .AsNoTracking()
            .Where(a => a.AccountNumber == accountNumber);

        if (ownerId is not null)
            query = query.Where(a => a.OwnerId == ownerId.Value);

        return await query
            .Select(a => new AccountBalanceDto(
                a.AccountNumber,
                a.Balance.Amount,
                a.Balance.Currency,
                a.Status.ToString()))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<AccountBalanceDto>> ListByOwner(Guid ownerId, CancellationToken ct)
    {
        return await _context.Accounts
            .AsNoTracking()
            .Where(a => a.OwnerId == ownerId)
            .OrderBy(a => a.AccountNumber)
            .Select(a => new AccountBalanceDto(
                a.AccountNumber,
                a.Balance.Amount,
                a.Balance.Currency,
                a.Status.ToString()))
            .ToListAsync(ct);
    }
}
