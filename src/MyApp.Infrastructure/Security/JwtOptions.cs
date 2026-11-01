using System.Text;
using Microsoft.Extensions.Configuration;

namespace MyApp.Infrastructure.Security;

public sealed record JwtOptions(string Issuer, string Audience, string SigningKey, int ExpiryMinutes)
{
    public static JwtOptions FromConfiguration(IConfiguration config)
    {
        var key = config["Jwt:SigningKey"];
        if (string.IsNullOrWhiteSpace(key) || Encoding.UTF8.GetByteCount(key) < 32)
            throw new InvalidOperationException(
                "Jwt:SigningKey is missing or shorter than 32 bytes. " +
                "Set it with: dotnet user-secrets set \"Jwt:SigningKey\" \"<long random value>\"");

        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];
        if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("Jwt:Issuer and Jwt:Audience must be configured.");

        var expiry = int.TryParse(config["Jwt:ExpiryMinutes"], out var minutes) && minutes > 0 ? minutes : 30;

        return new JwtOptions(issuer, audience, key, expiry);
    }
}
