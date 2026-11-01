using MyApp.Domain.Common;
using Shouldly;
using Xunit;

namespace MyApp.Domain.Tests;

public class InterestRateTests
{
    [Fact]
    public void FromPercent_stores_a_decimal_fraction()
    {
        InterestRate.FromPercent(2m).Annual.ShouldBe(0.02m);
        InterestRate.FromPercent(12m).Annual.ShouldBe(0.12m);
        InterestRate.FromPercent(0m).IsZero.ShouldBeTrue();
        InterestRate.Zero.IsZero.ShouldBeTrue();
    }

    [Theory]
    [InlineData("-0.01")]
    [InlineData("100.01")]
    public void FromPercent_rejects_rates_outside_0_to_100_percent(string percent)
        => Should.Throw<ValidationFailedException>(
            () => InterestRate.FromPercent(decimal.Parse(percent, System.Globalization.CultureInfo.InvariantCulture)));

    // Section 9.1, example 1: 10,000.00 at 2.00% -> 0.547945 per day.
    [Fact]
    public void Daily_credit_accrual_matches_the_savings_example()
        => InterestRate.FromPercent(2m).DailyAccrual(Money.Of(10_000m)).ShouldBe(0.547945m);

    // Section 9.1, example 2: -500.00 at 12.00% -> 0.164384 per day (the magnitude is used).
    [Fact]
    public void Daily_overdraft_accrual_uses_the_magnitude_of_a_negative_balance()
        => InterestRate.FromPercent(12m).DailyAccrual(Money.Of(-500m)).ShouldBe(0.164384m);

    [Fact]
    public void A_zero_rate_or_zero_balance_accrues_nothing()
    {
        InterestRate.Zero.DailyAccrual(Money.Of(10_000m)).ShouldBe(0m);
        InterestRate.FromPercent(2m).DailyAccrual(Money.Zero).ShouldBe(0m);
    }

    // Section 9.1, example 1 end to end: 31 days accumulate, post rounded, residual carried.
    [Fact]
    public void Savings_month_posts_16_99_and_carries_a_small_negative_residual()
    {
        var daily = InterestRate.FromPercent(2m).DailyAccrual(Money.Of(10_000m));

        var accrued = daily * 31;
        var posted = Money.RoundHalfEven(accrued);
        var residual = accrued - posted.Amount;

        accrued.ShouldBe(16.986295m);
        posted.ShouldBe(Money.Of(16.99m));
        residual.ShouldBe(-0.003705m);
    }

    // Section 9.1, example 2 end to end.
    [Fact]
    public void Overdraft_month_posts_1_64_and_carries_a_small_positive_residual()
    {
        var daily = InterestRate.FromPercent(12m).DailyAccrual(Money.Of(-500m));

        var accrued = daily * 10;
        var posted = Money.RoundHalfEven(accrued);

        accrued.ShouldBe(1.64384m);
        posted.ShouldBe(Money.Of(1.64m));
        (accrued - posted.Amount).ShouldBe(0.00384m);
    }

    // Section 9.1, example 3: the balance changes mid-month, so each day uses that day's balance.
    [Fact]
    public void A_changing_balance_accrues_day_by_day()
    {
        var rate = InterestRate.FromPercent(2m);

        var accrued = rate.DailyAccrual(Money.Of(1_000m)) * 15 + rate.DailyAccrual(Money.Of(3_000m)) * 15;

        accrued.ShouldBe(3.287685m);
        Money.RoundHalfEven(accrued).ShouldBe(Money.Of(3.29m));
    }

    [Fact]
    public void Accrual_always_has_at_most_six_decimals()
    {
        var random = new Random(99);
        var rate = InterestRate.FromPercent(2.37m);

        for (var i = 0; i < 500; i++)
        {
            var balance = Money.Of(random.Next(-5_000_000, 5_000_000) / 100m);
            var accrual = rate.DailyAccrual(balance);

            decimal.Round(accrual, 6).ShouldBe(accrual);
        }
    }
}
