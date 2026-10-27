namespace MyApp.Application.BankAccounts.Commands.DepositMoney;

public record DepositMoneyCommand(string AccountNumber, decimal Amount, string Currency, string? Description);
