using MyApp.Domain.Common;
using Shouldly;
using Xunit;

namespace MyApp.Domain.Tests;

public class MoneyTests
{
    [Fact]
    public void Of_accepts_two_decimals()
        => Money.Of(12.34m).Amount.ShouldBe(12.34m);

    [Fact]
    public void Of_accepts_trailing_zeros()
        => Money.Of(1.500m).Amount.ShouldBe(1.5m);

    [Theory]
    [InlineData("0.001")]
    [InlineData("1.005")]
    [InlineData("-2.345")]
    public void Of_rejects_more_than_two_decimals_instead_of_rounding(string text)
        => Should.Throw<ValidationFailedException>(() => Money.Of(decimal.Parse(text, System.Globalization.CultureInfo.InvariantCulture)));

    [Fact]
    public void Of_accepts_the_maximum_magnitude_in_both_signs()
    {
        Money.Of(999_999_999_999.99m).Amount.ShouldBe(999_999_999_999.99m);
        Money.Of(-999_999_999_999.99m).Amount.ShouldBe(-999_999_999_999.99m);
    }

    [Fact]
    public void Of_rejects_amounts_beyond_the_maximum()
        => Should.Throw<ValidationFailedException>(() => Money.Of(1_000_000_000_000m));

    [Fact]
    public void Money_is_signed_so_balances_can_be_negative()
    {
        var overdrawn = Money.Of(-450m);

        overdrawn.IsNegative.ShouldBeTrue();
        overdrawn.IsPositive.ShouldBeFalse();
        overdrawn.IsZero.ShouldBeFalse();
    }

    [Fact]
    public void Zero_is_zero_and_default_is_zero()
    {
        Money.Zero.IsZero.ShouldBeTrue();
        default(Money).ShouldBe(Money.Zero);
    }

    [Fact]
    public void Subtracting_a_larger_amount_gives_a_negative_result()
        => (Money.Of(100m) - Money.Of(150m)).ShouldBe(Money.Of(-50m));

    [Fact]
    public void Addition_beyond_the_maximum_is_rejected()
        => Should.Throw<ValidationFailedException>(() => Money.Of(Money.MaxMagnitude) + Money.Of(0.01m));

    [Fact]
    public void Negation_and_abs_work()
    {
        (-Money.Of(5m)).ShouldBe(Money.Of(-5m));
        Money.Of(-5m).Abs().ShouldBe(Money.Of(5m));
    }

    [Fact]
    public void Comparison_operators_follow_the_amount()
    {
        (Money.Of(1m) < Money.Of(2m)).ShouldBeTrue();
        (Money.Of(2m) > Money.Of(1m)).ShouldBeTrue();
        (Money.Of(2m) <= Money.Of(2m)).ShouldBeTrue();
        (Money.Of(2m) >= Money.Of(3m)).ShouldBeFalse();
        Money.Of(-1m).CompareTo(Money.Zero).ShouldBeLessThan(0);
    }

    [Fact]
    public void Equality_is_by_value_regardless_of_trailing_zeros()
    {
        Money.Of(1.5m).ShouldBe(Money.Of(1.50m));
        (Money.Of(1.5m) == Money.Of(1.500m)).ShouldBeTrue();
        Money.Of(1.5m).GetHashCode().ShouldBe(Money.Of(1.50m).GetHashCode());
    }

    // P-12: round half to even to 2 decimals. The first four rows are the examples from section 9.
    [Theory]
    [InlineData("16.986295", "16.99")]
    [InlineData("1.643840", "1.64")]
    [InlineData("3.287685", "3.29")]
    [InlineData("2.345000", "2.34")]
    [InlineData("2.355", "2.36")]
    [InlineData("0.125", "0.12")]
    [InlineData("0.135", "0.14")]
    [InlineData("-2.345", "-2.34")]
    public void RoundHalfEven_rounds_ties_to_the_even_neighbour(string input, string expected)
    {
        var value = decimal.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
        var want = decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture);

        Money.RoundHalfEven(value).Amount.ShouldBe(want);
    }

    [Fact]
    public void ToString_always_shows_two_decimals_with_a_dot()
    {
        Money.Of(5m).ToString().ShouldBe("5.00");
        Money.Of(-3m).ToString().ShouldBe("-3.00");
        Money.Of(0.5m).ToString().ShouldBe("0.50");
    }

    // Property-style check with a fixed seed: adding then subtracting returns the original.
    [Fact]
    public void Adding_then_subtracting_returns_the_original_for_many_random_pairs()
    {
        var random = new Random(20261101);

        for (var i = 0; i < 1000; i++)
        {
            var a = Money.Of(random.Next(-5_000_000, 5_000_000) / 100m);
            var b = Money.Of(random.Next(-5_000_000, 5_000_000) / 100m);

            ((a + b) - b).ShouldBe(a);
        }
    }
}
