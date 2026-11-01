using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.BankAccounts.Commands.CloseAccount;
using MyApp.Application.BankAccounts.Commands.DepositMoney;
using MyApp.Application.BankAccounts.Commands.OpenAccount;
using MyApp.Application.BankAccounts.Commands.ReactivateAccount;
using MyApp.Application.BankAccounts.Commands.SuspendAccount;
using MyApp.Application.BankAccounts.Commands.WithdrawMoney;
using MyApp.Application.BankAccounts.Queries.GetAccountBalance;
using MyApp.Application.BankAccounts.Queries.GetMyAccounts;
using MyApp.Application.Users.Commands.ActivateUser;
using MyApp.Application.Users.Commands.Login;
using MyApp.Application.Users.Commands.RegisterUser;
using MyApp.Application.Users.Commands.ReinstateUser;
using MyApp.Application.Users.Commands.SuspendUser;
using MyApp.Application.Users.Queries.GetCurrentUser;
using MyApp.Application.Users.Queries.ListUsers;

namespace MyApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Users
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<ActivateUserHandler>();
        services.AddScoped<SuspendUserHandler>();
        services.AddScoped<ReinstateUserHandler>();
        services.AddScoped<GetCurrentUserHandler>();
        services.AddScoped<ListUsersHandler>();

        // Bank accounts
        services.AddScoped<OpenAccountHandler>();
        services.AddScoped<DepositMoneyHandler>();
        services.AddScoped<WithdrawMoneyHandler>();
        services.AddScoped<SuspendAccountHandler>();
        services.AddScoped<ReactivateAccountHandler>();
        services.AddScoped<CloseAccountHandler>();
        services.AddScoped<GetAccountBalanceHandler>();
        services.AddScoped<GetMyAccountsHandler>();

        return services;
    }
}
