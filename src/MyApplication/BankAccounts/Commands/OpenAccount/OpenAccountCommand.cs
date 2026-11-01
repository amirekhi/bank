namespace MyApp.Application.BankAccounts.Commands.OpenAccount;

// The owner is always the authenticated caller; the client can no longer choose it.
public record OpenAccountCommand(string AccountNumber, string Currency);
