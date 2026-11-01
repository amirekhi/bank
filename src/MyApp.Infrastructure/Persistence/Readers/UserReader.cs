// MyApp.Infrastructure/Persistence/Readers/UserReader.cs
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Common;
using MyApp.Application.Users.Queries.ListUsers;
using MyApp.Domain.Users;

namespace MyApp.Infrastructure.Persistence.Readers;

public class UserReader : IUserReader
{
    private readonly AppDbContext _context;

    public UserReader(AppDbContext context) => _context = context;

    public async Task<PagedResult<UserSummaryDto>> List(UserStatus? status, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.Users.AsNoTracking();

        if (status is not null)
        {
            var wanted = status.Value;
            query = query.Where(u => u.Status == wanted);
        }

        var total = await query.CountAsync(ct);

        var rows = await query
            .OrderBy(u => u.Name).ThenBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new { u.Id, u.Name, Email = u.Email.Value, u.Role, u.Status })
            .ToListAsync(ct);

        var items = rows
            .Select(r => new UserSummaryDto(r.Id, r.Name, r.Email, r.Role.ToString(), r.Status.ToString()))
            .ToList();

        return new PagedResult<UserSummaryDto>(items, page, pageSize, total);
    }
}
