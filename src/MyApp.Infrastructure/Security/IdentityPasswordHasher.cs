using Microsoft.AspNetCore.Identity;
using MyApp.Application.Abstractions;

namespace MyApp.Infrastructure.Security;

/// <summary>Uses ASP.NET Core Identity's PBKDF2 hasher (salted, versioned, upgradeable) without the rest of Identity.</summary>
public sealed class IdentityPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password) => _hasher.HashPassword(null!, password);

    public bool Verify(string hash, string password)
        => _hasher.VerifyHashedPassword(null!, hash, password) != PasswordVerificationResult.Failed;
}
