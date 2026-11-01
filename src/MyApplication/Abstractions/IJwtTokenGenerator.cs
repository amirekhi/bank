using MyApp.Domain.Users;

namespace MyApp.Application.Abstractions;

public record AccessToken(string Token, DateTime ExpiresAtUtc);

public interface IJwtTokenGenerator
{
    AccessToken Generate(User user);
}
