namespace MyApp.Application.BankAccounts.Queries.GetAccountBalance;

public record AccountBalanceDto(string AccountNumber, decimal Balance, string Currency, string Status);
