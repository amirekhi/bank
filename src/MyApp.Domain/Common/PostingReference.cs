using System.Globalization;
using System.Text.RegularExpressions;

namespace MyApp.Domain.Common;

/// <summary>Unique, human-readable posting reference PST-yyyymmdd-nnnnnn: business date plus a daily sequence (T-06).</summary>
public sealed partial record PostingReference
{
    public const int MaxDailySequence = 999_999;

    private PostingReference(string value) => Value = value;

    public string Value { get; }

    public static PostingReference Create(BusinessDate date, int dailySequence)
    {
        if (dailySequence < 1 || dailySequence > MaxDailySequence)
            throw new ValidationFailedException("The daily posting sequence is out of range.");

        var day = date.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var number = dailySequence.ToString("D6", CultureInfo.InvariantCulture);
        return new PostingReference($"PST-{day}-{number}");
    }

    public static PostingReference Parse(string? value)
    {
        var match = value is null ? null : Pattern().Match(value);

        if (match is null || !match.Success
            || !DateOnly.TryParseExact(match.Groups[1].Value, "yyyyMMdd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _)
            || int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture) < 1)
        {
            throw new ValidationFailedException("The posting reference is not valid.");
        }

        return new PostingReference(value!);
    }

    [GeneratedRegex(@"^PST-(\d{8})-(\d{6})$")]
    private static partial Regex Pattern();

    public override string ToString() => Value;
}
