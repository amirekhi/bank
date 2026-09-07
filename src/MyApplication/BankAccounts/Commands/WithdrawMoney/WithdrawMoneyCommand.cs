using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// MyApp.Application/BankAccounts/Commands/WithdrawMoney/WithdrawMoneyCommand.cs
namespace MyApplication.BankAccounts.Commands.WithdrawMoney;

public record WithdrawMoneyCommand(string AccountNumber, decimal Amount, string Currency, string? Description);