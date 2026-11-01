using System.Globalization;

namespace MyApp.Domain.Common;

/// <summary>
/// A UTC calendar date with no time part (P-24). Business date of a posting is the UTC date of its timestamp (T-07).
/// </summary>
public readonly record struct BusinessDate : IComparable<BusinessDate>
{
    public BusinessDate(DateOnly value) => Value = value;

    public DateOnly Value { get; }

    public static BusinessDate FromUtc(DateTimeOffset timestamp)
        => new(DateOnly.FromDateTime(timestamp.UtcDateTime));

    public static BusinessDate Today(TimeProvider clock)
        => FromUtc(clock.GetUtcNow());

    public BusinessDate AddDays(int days) => new(Value.AddDays(days));

    public bool IsLastDayOfMonth => Value.AddDays(1).Day == 1;

    /// <summary>Format yyyy-MM, used in descriptions such as "Interest 2026-10".</summary>
    public string YearMonth => Value.ToString("yyyy-MM", CultureInfo.InvariantCulture);

    public int CompareTo(BusinessDate other) => Value.CompareTo(other.Value);

    public static bool operator <(BusinessDate left, BusinessDate right) => left.Value < right.Value;
    public static bool operator >(BusinessDate left, BusinessDate right) => left.Value > right.Value;
    public static bool operator <=(BusinessDate left, BusinessDate right) => left.Value <= right.Value;
    public static bool operator >=(BusinessDate left, BusinessDate right) => left.Value >= right.Value;

    public override string ToString() => Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
