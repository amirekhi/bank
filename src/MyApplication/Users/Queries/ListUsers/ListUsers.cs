using MyApp.Application.Common;
using MyApp.Domain.Users;

namespace MyApp.Application.Users.Queries.ListUsers;

public record ListUsersQuery(UserStatus? Status, int Page = 1, int PageSize = 20);

public record UserSummaryDto(Guid Id, string Name, string Email, string Role, string Status);

public interface IUserReader
{
    Task<PagedResult<UserSummaryDto>> List(UserStatus? status, int page, int pageSize, CancellationToken ct);
}

public class ListUsersHandler
{
    private readonly IUserReader _reader;

    public ListUsersHandler(IUserReader reader) => _reader = reader;

    public Task<PagedResult<UserSummaryDto>> Handle(ListUsersQuery query, CancellationToken ct)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        return _reader.List(query.Status, page, pageSize, ct);
    }
}
