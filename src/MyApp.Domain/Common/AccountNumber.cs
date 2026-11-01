using System.Globalization;

namespace MyApp.Domain.Common;

/// <summary>
/// Ten digits: a 9-digit sequence starting at 100000000 followed by one Luhn check digit (A-08, P-29).
/// Customers never choose it; it is always generated from a database sequence.
/// </summary>
public sealed record AccountNumber
{
    public const long FirstSequence = 100_000_000;
    public const long LastSequence = 999_999_999;

    private AccountNumber(string value) => Value = value;

    public string Value { get; }

    public static AccountNumber FromSequence(long sequence)
    {
        if (sequence < FirstSequence || sequence > LastSequence)
            throw new ValidationFailedException("The account number sequence is out of range.");

        var payload = sequence.ToString(CultureInfo.InvariantCulture);
        return new AccountNumber(payload + Luhn.ComputeCheckDigit(payload));
    }

    public static AccountNumber Parse(string? value)
    {
        if (!TryParse(value, out var number))
            throw new ValidationFailedException("The account number is not valid.");

        return number!;
    }

    public static bool TryParse(string? value, out AccountNumber? number)
    {
        number = null;

        if (value is null || value.Length != 10 || value[0] == '0' || !Luhn.IsValid(value))
            return false;

        number = new AccountNumber(value);
        return true;
    }

    public override string ToString() => Value;
}
