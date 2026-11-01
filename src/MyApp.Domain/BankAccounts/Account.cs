using MyApp.Domain.BankAccounts.Exceptions;
using MyApp.Domain.Common;

namespace MyApp.Domain.BankAccounts
{
    public class Account : Entity
    {
        private readonly List<Transaction> _transactions = new();
        public string AccountNumber { get; private set; }
        public Money Balance { get; private set; }
        public Money OverdraftLimit { get; private set; }
        public AccountStatus Status { get; private set; }

        public Guid OwnerId { get; private set; }
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private Account() { }

        public Account(string accountNumber, Guid ownerId, string currency, Money? overdraftLimit = null)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required.");

            Id = Guid.NewGuid();
            AccountNumber = accountNumber;
            OwnerId = ownerId;
            Balance = Money.Zero(currency);
            OverdraftLimit = overdraftLimit ?? Money.Zero(currency);
            Status = AccountStatus.Active;
        }

        public bool IsOwnedBy(Guid userId) => OwnerId == userId;

        public void Deposit(Money amount, string? description = null)
        {
            EnsureActive();
            if (amount.Amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");

            Balance = Balance.Add(amount);
            _transactions.Add(new Transaction(TransactionType.Deposit, amount, description));
        }

        public void Withdraw(Money amount, string? description = null)
        {
            EnsureActive();
            if (amount.Amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive.");

            var minimumAllowedBalance = Money.Zero(Balance.Currency).Subtract(OverdraftLimit);
            var balanceAfterWithdrawal = Balance.Subtract(amount);

            if (!balanceAfterWithdrawal.IsGreaterThanOrEqualTo(minimumAllowedBalance))
                throw new InsufficientFundsException(AccountNumber, amount, Balance);

            Balance = balanceAfterWithdrawal;
            _transactions.Add(new Transaction(TransactionType.Withdrawal, amount, description));
        }

        public void Suspend()
        {
            if (Status != AccountStatus.Active)
                throw new InvalidStatusTransitionException($"Account {AccountNumber} cannot be suspended from status {Status}.");
            Status = AccountStatus.Suspended;
        }

        public void Reactivate()
        {
            if (Status != AccountStatus.Suspended)
                throw new InvalidStatusTransitionException($"Account {AccountNumber} cannot be reactivated from status {Status}.");
            Status = AccountStatus.Active;
        }

        public void Close()
        {
            if (Status == AccountStatus.Closed)
                throw new AccountClosedException(AccountNumber);
            if (Balance.Amount > 0)
                throw new InvalidOperationException("Cannot close an account with a non-zero balance.");

            Status = AccountStatus.Closed;
        }

        private void EnsureActive()
        {
            if (Status == AccountStatus.Closed)
                throw new AccountClosedException(AccountNumber);
            if (Status == AccountStatus.Suspended)
                throw new AccountSuspendedException(AccountNumber);
        }
    }
}
