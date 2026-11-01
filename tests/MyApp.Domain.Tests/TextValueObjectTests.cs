using MyApp.Domain.Common;
using Shouldly;
using Xunit;

namespace MyApp.Domain.Tests;

public class IdempotencyKeyTests
{
    [Theory]
    [InlineData("abcd1234")]
    [InlineData("7f9c2b1e-4d3a-4f6e-9a1b-0c2d3e4f5a6b")]
    [InlineData("order_2026.11:01-a")]
    public void Accepts_safe_keys(string key)
        => IdempotencyKey.Parse(key).Value.ShouldBe(key);

    [Fact]
    public void Accepts_exactly_the_minimum_and_maximum_length()
    {
        IdempotencyKey.Parse(new string('a', 8)).Value.Length.ShouldBe(8);
        IdempotencyKey.Parse(new string('a', 64)).Value.Length.ShouldBe(64);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("1234567")]
    public void Rejects_keys_that_are_too_short(string? key)
        => Should.Throw<ValidationFailedException>(() => IdempotencyKey.Parse(key));

    [Fact]
    public void Rejects_keys_that_are_too_long()
        => Should.Throw<ValidationFailedException>(() => IdempotencyKey.Parse(new string('a', 65)));

    [Theory]
    [InlineData("has space 123")]
    [InlineData("path/like/key1")]
    [InlineData("quote'd-key-1")]
    [InlineData("umlaut-\u00e4-key1")]
    public void Rejects_keys_with_unsafe_characters(string key)
        => Should.Throw<ValidationFailedException>(() => IdempotencyKey.Parse(key));

    [Fact]
    public void Keys_are_equal_by_value()
        => IdempotencyKey.Parse("abcd1234").ShouldBe(IdempotencyKey.Parse("abcd1234"));
}

public class ReasonTests
{
    [Fact]
    public void Trims_surrounding_whitespace()
        => Reason.Parse("   Customer asked by phone.   ").Value.ShouldBe("Customer asked by phone.");

    [Fact]
    public void Accepts_exactly_the_minimum_and_maximum_length()
    {
        Reason.Parse(new string('x', 10)).Value.Length.ShouldBe(10);
        Reason.Parse(new string('x', 500)).Value.Length.ShouldBe(500);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("          ")]
    [InlineData("too short")]
    public void Rejects_blank_or_too_short_reasons(string? text)
        => Should.Throw<ValidationFailedException>(() => Reason.Parse(text));

    [Fact]
    public void Length_is_measured_after_trimming()
        => Should.Throw<ValidationFailedException>(() => Reason.Parse("   short   "));

    [Fact]
    public void Rejects_reasons_that_are_too_long()
        => Should.Throw<ValidationFailedException>(() => Reason.Parse(new string('x', 501)));
}

public class PostingReferenceTests
{
    [Fact]
    public void Create_combines_the_business_date_and_a_six_digit_daily_sequence()
    {
        var date = new BusinessDate(new DateOnly(2026, 11, 1));
        PostingReference.Create(date, 7).Value.ShouldBe("PST-20261101-000007");
        PostingReference.Create(date, 999_999).Value.ShouldBe("PST-20261101-999999");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(1_000_000)]
    public void Create_rejects_a_sequence_outside_1_to_999999(int sequence)
        => Should.Throw<ValidationFailedException>(
            () => PostingReference.Create(new BusinessDate(new DateOnly(2026, 11, 1)), sequence));

    [Fact]
    public void Parse_accepts_a_created_reference()
    {
        var created = PostingReference.Create(new BusinessDate(new DateOnly(2026, 2, 28)), 42);
        PostingReference.Parse(created.Value).ShouldBe(created);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("TRX-20261101-000007")]
    [InlineData("PST-20261101-7")]
    [InlineData("PST-2026110-000007")]
    [InlineData("PST-20261301-000007")]
    [InlineData("PST-20261101-000000")]
    [InlineData("pst-20261101-000007")]
    public void Parse_rejects_malformed_references(string? text)
        => Should.Throw<ValidationFailedException>(() => PostingReference.Parse(text));
}
