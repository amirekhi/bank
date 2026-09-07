// MyApp.Domain/BankAccounts/Exceptions/InsufficientFundsException.cs
using System;

namespace MyApp.Domain.BankAccounts.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string accountNumber, Money attempted, Money balance)
            : base($"Account {accountNumber} has insufficient funds. Attempted: {attempted}, Balance: {balance}") { }
    }
}