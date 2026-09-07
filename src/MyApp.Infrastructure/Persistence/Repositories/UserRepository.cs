// MyApp.Infrastructure/Persistence/Repositories/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Users;

namespace MyApp.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetById(Guid id, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmail(string email, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email.ToLower(), ct);

    public async Task Save(User user, CancellationToken ct)
    {
        if (_context.Entry(user).State == EntityState.Detached)
            _context.Users.Add(user);

        await _context.SaveChangesAsync(ct);
    }
}