namespace MyApp.Application.BankAccounts.Commands.WithdrawMoney;

public record WithdrawMoneyCommand(string AccountNumber, decimal Amount, string Currency, string? Description);
