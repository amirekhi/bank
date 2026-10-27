using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.BankAccounts.Commands.DepositMoney;
using MyApp.Application.BankAccounts.Commands.OpenAccount;
using MyApp.Application.BankAccounts.Commands.WithdrawMoney;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;
using MyApp.Application.Users.Commands.RegisterUser;

namespace MyApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<DepositMoneyHandler>();
        services.AddScoped<WithdrawMoneyHandler>();
        services.AddScoped<OpenAccountHandler>();
        services.AddScoped<GetAccountBalanceHandler>();
        return services;
    }
}
