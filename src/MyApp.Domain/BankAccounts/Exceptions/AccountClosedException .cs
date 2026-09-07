using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Domain.BankAccounts.Exceptions
{
    public class AccountClosedException : Exception
    {
        public AccountClosedException (string accountNumber) 
        : base ($"Account {accountNumber} is closed and cannot process transactions.") { }

    }
}