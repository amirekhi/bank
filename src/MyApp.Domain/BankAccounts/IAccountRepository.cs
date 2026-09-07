using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Domain.BankAccounts
{
    public interface IAccountRepository
    {
      Task<Account?> GetByAccountNumber(string accountNumber, CancellationToken ct);
      Task Save(Account account, CancellationToken ct);
    }
}