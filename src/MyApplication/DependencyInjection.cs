// MyApp.Application/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;

using MyApp.Application.BankAccounts.Queries.GetAccountBalance;
using MyApp.Application.Users.Commands.RegisterUser;

using MyApplication.BankAccounts.Commands.DepositMoney;
using MyApplication.BankAccounts.Commands.WithdrawMoney;
using MyApplication.BankAccounts.Commands.OpenAccount;

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