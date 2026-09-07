using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Domain.BankAccounts
{
    public class Money : IEquatable<Money>
    {

        public decimal Amount  {get;} 
        public string Currency {get;}

        public Money(decimal amount , string currency)
        {
            if(amount < 0 )
            {
                throw new ArgumentException("the amount cannot be less than 0");
            }else if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("currency can not be null or whitespace");
            }


            Amount = amount;
            Currency = currency.ToUpperInvariant();

        }


        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount , Currency);
        }


        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount - other.Amount , Currency );
        }

        public bool IsGreaterThanOrEqualTo(Money other)
        {
            EnsureSameCurrency(other);
            return Amount >= other.Amount;
        }


        private void EnsureSameCurrency(Money other)
        {
            if(other.Currency != Currency)
            {
                throw new InvalidOperationException($"currency mismatch {other.Currency} vs {Currency}");
            }
        }


        public static Money Zero(string currency)
        {
            return new Money(0 , currency);
        } 

        public bool Equals(Money? other) =>
            other is not null && Amount == other.Amount && Currency == other.Currency;

        public override bool Equals(object? obj) => Equals(obj as Money);
        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
        public override string ToString() => $"{Amount} {Currency}";

    }


}