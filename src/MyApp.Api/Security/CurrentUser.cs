using System.Security.Claims;
using MyApp.Application.Abstractions;

namespace MyApp.Api.Security;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal Principal =>
        _accessor.HttpContext?.User is { Identity.IsAuthenticated: true } principal
            ? principal
            : throw new UnauthorizedAccessException("No authenticated user.");

    public Guid UserId =>
        Guid.TryParse(Principal.FindFirst("sub")?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException("Token has no valid 'sub' claim.");

    public bool IsAdmin => Principal.IsInRole("Admin");
}
