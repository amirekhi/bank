using MyApp.Domain.Common;
using Shouldly;
using Xunit;

namespace MyApp.Domain.Tests;

public class BusinessDateTests
{
    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }

    [Fact]
    public void Business_date_is_the_UTC_date_of_the_timestamp()
    {
        var justBeforeMidnightUtc = new DateTimeOffset(2026, 10, 31, 23, 59, 59, TimeSpan.Zero);
        BusinessDate.FromUtc(justBeforeMidnightUtc).Value.ShouldBe(new DateOnly(2026, 10, 31));
    }

    [Fact]
    public void A_timestamp_with_an_offset_is_converted_to_UTC_first()
    {
        // 01:00 on 1 November at +02:00 is 23:00 on 31 October UTC.
        var local = new DateTimeOffset(2026, 11, 1, 1, 0, 0, TimeSpan.FromHours(2));
        BusinessDate.FromUtc(local).Value.ShouldBe(new DateOnly(2026, 10, 31));
    }

    [Fact]
    public void Today_reads_the_injected_clock()
    {
        var clock = new FixedTimeProvider(new DateTimeOffset(2026, 11, 1, 0, 10, 0, TimeSpan.Zero));
        BusinessDate.Today(clock).Value.ShouldBe(new DateOnly(2026, 11, 1));
    }

    [Theory]
    [InlineData(2026, 10, 31, true)]
    [InlineData(2026, 10, 30, false)]
    [InlineData(2028, 2, 29, true)]
    [InlineData(2026, 2, 28, true)]
    [InlineData(2026, 2, 27, false)]
    [InlineData(2026, 12, 31, true)]
    public void IsLastDayOfMonth_handles_month_lengths_and_leap_years(int year, int month, int day, bool expected)
        => new BusinessDate(new DateOnly(year, month, day)).IsLastDayOfMonth.ShouldBe(expected);

    [Fact]
    public void AddDays_rolls_over_month_and_year()
    {
        new BusinessDate(new DateOnly(2026, 12, 31)).AddDays(1).Value.ShouldBe(new DateOnly(2027, 1, 1));
        new BusinessDate(new DateOnly(2026, 3, 1)).AddDays(-1).Value.ShouldBe(new DateOnly(2026, 2, 28));
    }

    [Fact]
    public void YearMonth_is_formatted_for_descriptions()
        => new BusinessDate(new DateOnly(2026, 10, 31)).YearMonth.ShouldBe("2026-10");

    [Fact]
    public void Dates_compare_in_calendar_order()
    {
        var earlier = new BusinessDate(new DateOnly(2026, 10, 31));
        var later = new BusinessDate(new DateOnly(2026, 11, 1));

        (earlier < later).ShouldBeTrue();
        (later > earlier).ShouldBeTrue();
        (earlier <= earlier).ShouldBeTrue();
        later.CompareTo(earlier).ShouldBeGreaterThan(0);
    }

    [Fact]
    public void ToString_is_ISO_formatted()
        => new BusinessDate(new DateOnly(2026, 1, 5)).ToString().ShouldBe("2026-01-05");
}
