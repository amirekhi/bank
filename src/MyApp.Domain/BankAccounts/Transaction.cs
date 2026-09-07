using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Domain.BankAccounts
{
    public class Transaction
    {
        public Guid Id {get; private set;}
        public TransactionType Type {get; private set;}
        public Money Amount {get; private set;}
        public DateTime OccurredAt {get; private set;}
        public string? Description {get; private set;}


        private Transaction() { } 


    internal Transaction(TransactionType type, Money amount, string? description)
    {
        Id = Guid.NewGuid();
        Type = type;
        Amount = amount;
        OccurredAt = DateTime.UtcNow;
        Description = description;
    }
    }
}