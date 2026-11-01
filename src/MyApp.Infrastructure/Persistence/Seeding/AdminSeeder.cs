using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.Abstractions;
using MyApp.Application.Users;
using MyApp.Domain.Users;

namespace MyApp.Infrastructure.Persistence.Seeding;

public static class AdminSeeder
{
    /// <summary>
    /// Creates the first admin from configuration (Seed:Admin:Email / Password / Name) if it does not exist.
    /// Does nothing when the settings are absent, e.g. in production.
    /// </summary>
    public static async Task SeedAdminAsync(this IServiceProvider services, CancellationToken ct = default)
    {
        using var scope = services.CreateScope();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var email = config["Seed:Admin:Email"];
        var password = config["Seed:Admin:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        PasswordPolicy.EnsureValid(password);

        var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var normalizedEmail = new Email(email);

        if (await users.GetByEmail(normalizedEmail.Value, ct) is not null)
            return;

        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var name = config["Seed:Admin:Name"] ?? "Administrator";

        var admin = User.CreateAdmin(name, normalizedEmail, hasher.Hash(password));
        await users.Save(admin, ct);
    }
}
