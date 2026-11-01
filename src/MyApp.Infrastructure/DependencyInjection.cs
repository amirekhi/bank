using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyApp.Application.Abstractions;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;
using MyApp.Application.Users.Queries.ListUsers;
using MyApp.Domain.BankAccounts;
using MyApp.Domain.Users;
using MyApp.Infrastructure.Persistence;
using MyApp.Infrastructure.Persistence.Readers;
using MyApp.Infrastructure.Persistence.Repositories;
using MyApp.Infrastructure.Security;

namespace MyApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Persistence
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountReader, AccountReader>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserReader, UserReader>();

        // Security
        services.TryAddSingleton(TimeProvider.System);
        services.AddSingleton(JwtOptions.FromConfiguration(config));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, IdentityPasswordHasher>();

        return services;
    }
}
