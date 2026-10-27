namespace MyApp.Application.BankAccounts.Commands.OpenAccount;

public record OpenAccountCommand(string AccountNumber, Guid OwnerId, string Currency, decimal OverdraftLimit = 0);
