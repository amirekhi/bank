// MyApp.Infrastructure/Persistence/Repositories/AccountRepository.cs
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.BankAccounts;

namespace MyApp.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context) => _context = context;

    public async Task<Account?> GetByAccountNumber(string accountNumber, CancellationToken ct)
    {
        return await _context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct);
    }

    public async Task Save(Account account, CancellationToken ct)
    {
        if (_context.Entry(account).State == EntityState.Detached)
            _context.Accounts.Add(account);

        await _context.SaveChangesAsync(ct);
    }
}