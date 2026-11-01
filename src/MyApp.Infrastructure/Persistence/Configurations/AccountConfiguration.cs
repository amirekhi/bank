// MyApp.Infrastructure/Persistence/Configurations/AccountConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.BankAccounts;
using MyApp.Domain.Users;

namespace MyApp.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.AccountNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(a => a.AccountNumber).IsUnique();

        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);

        // Cross-aggregate reference by id only (no navigation property), enforced by a foreign key.
        builder.HasIndex(a => a.OwnerId);
        builder.HasOne<User>().WithMany().HasForeignKey(a => a.OwnerId).OnDelete(DeleteBehavior.Restrict);

        // Money is a value object -> map as owned type (no separate table, just extra columns)
        builder.OwnsOne(a => a.Balance, m =>
        {
            m.Property(p => p.Amount).HasColumnName("Balance").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
        });

        builder.OwnsOne(a => a.OverdraftLimit, m =>
        {
            m.Property(p => p.Amount).HasColumnName("OverdraftLimit").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("OverdraftCurrency").HasMaxLength(3);
        });

        // Access the private _transactions field, not a public setter
        builder.HasMany(a => a.Transactions)
               .WithOne()
               .HasForeignKey("AccountId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata.FindNavigation(nameof(Account.Transactions))!
               .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        // The id is generated in the domain. Without this, EF sees a non-empty key on a new
        // child and assumes the row already exists (UPDATE instead of INSERT).
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);

        builder.OwnsOne(t => t.Amount, m =>
        {
            m.Property(p => p.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
        });
    }
}
