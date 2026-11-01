using System.Globalization;

namespace MyApp.Domain.Common;

/// <summary>
/// An amount in the single system currency (P-01). Signed, exactly 2 decimal places (5.1).
/// Positivity is enforced by the operations that use it, not by this type.
/// </summary>
public readonly record struct Money : IComparable<Money>
{
    public const decimal MaxMagnitude = 999_999_999_999.99m;

    private Money(decimal amount) => Amount = amount;

    public decimal Amount { get; }

    public static Money Zero => new(0m);

    /// <summary>
    /// Creates money from an exact amount. More than 2 decimals is rejected, never silently rounded (5.1).
    /// Trailing zeros are fine (1.500 is 1.50).
    /// </summary>
    public static Money Of(decimal amount)
    {
        if (decimal.Round(amount, 2, MidpointRounding.ToEven) != amount)
            throw new ValidationFailedException("An amount can have at most 2 decimal places.");

        return Create(amount);
    }

    /// <summary>Rounds a higher-precision value to 2 decimals, half to even (P-12). Used for interest posting.</summary>
    public static Money RoundHalfEven(decimal value)
        => Create(decimal.Round(value, 2, MidpointRounding.ToEven));

    private static Money Create(decimal amount)
    {
        if (Math.Abs(amount) > MaxMagnitude)
            throw new ValidationFailedException("The amount is outside the supported range.");

        return new Money(amount);
    }

    public bool IsZero => Amount == 0m;
    public bool IsPositive => Amount > 0m;
    public bool IsNegative => Amount < 0m;

    public Money Abs() => new(Math.Abs(Amount));

    public int CompareTo(Money other) => Amount.CompareTo(other.Amount);

    public static Money operator +(Money left, Money right) => Create(left.Amount + right.Amount);
    public static Money operator -(Money left, Money right) => Create(left.Amount - right.Amount);
    public static Money operator -(Money value) => new(-value.Amount);

    public static bool operator <(Money left, Money right) => left.Amount < right.Amount;
    public static bool operator >(Money left, Money right) => left.Amount > right.Amount;
    public static bool operator <=(Money left, Money right) => left.Amount <= right.Amount;
    public static bool operator >=(Money left, Money right) => left.Amount >= right.Amount;

    public override string ToString() => Amount.ToString("F2", CultureInfo.InvariantCulture);
}
