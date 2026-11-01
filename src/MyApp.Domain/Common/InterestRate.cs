namespace MyApp.Domain.Common;

/// <summary>
/// An annual interest rate stored as a decimal fraction (2.00% is 0.02), between 0% and 100%.
/// Daily accrual uses Actual/365 fixed with 6-decimal precision, half to even (P-11, P-12, I-02, I-03).
/// </summary>
public readonly record struct InterestRate
{
    public const int DaysInYear = 365;
    public const int AccrualDecimals = 6;

    private InterestRate(decimal annual) => Annual = annual;

    public decimal Annual { get; }

    public static InterestRate Zero => new(0m);

    public bool IsZero => Annual == 0m;

    public static InterestRate FromPercent(decimal percent)
    {
        if (percent < 0m || percent > 100m)
            throw new ValidationFailedException("An interest rate must be between 0% and 100%.");

        return new InterestRate(percent / 100m);
    }

    /// <summary>One day of interest on the magnitude of a balance: |balance| x rate / 365, rounded to 6 decimals.</summary>
    public decimal DailyAccrual(Money balance)
        => decimal.Round(Math.Abs(balance.Amount) * Annual / DaysInYear, AccrualDecimals, MidpointRounding.ToEven);
}
