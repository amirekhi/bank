namespace MyApp.Domain.BankAccounts.Exceptions
{
    public class AccountSuspendedException : Exception
    {
        public AccountSuspendedException(string accountNumber)
            : base($"Account {accountNumber} is suspended and cannot process transactions.") { }
    }
}
