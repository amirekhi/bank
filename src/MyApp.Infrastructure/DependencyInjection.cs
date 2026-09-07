// MyApp.Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;
using MyApp.Domain.BankAccounts;
using MyApp.Domain.Users;
using MyApp.Infrastructure.Persistence;
using MyApp.Infrastructure.Persistence.Readers;
using MyApp.Infrastructure.Persistence.Repositories;

namespace MyApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountReader, AccountReader>();

      
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}