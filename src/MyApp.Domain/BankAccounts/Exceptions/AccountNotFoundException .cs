using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Domain.BankAccounts.Exceptions
{
        public class AccountNotFoundException : Exception
        {
            public AccountNotFoundException(string accountNumber)
                : base($"Account {accountNumber} was not found.") { }
        }
}