using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication.BankAccounts.Commands.DepositMoney;

public record DepositMoneyCommand(string AccountNumber, decimal Amount, string Currency, string? Description);