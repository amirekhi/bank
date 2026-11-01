namespace MyApp.Domain.Common;

/// <summary>Luhn mod-10 check digit, used for account numbers (A-08, P-29).</summary>
public static class Luhn
{
    public static int ComputeCheckDigit(string payload)
    {
        if (string.IsNullOrEmpty(payload) || !payload.All(char.IsAsciiDigit))
            throw new ValidationFailedException("The Luhn payload must contain digits only.");

        var sum = 0;
        var doubleIt = true; // the rightmost payload digit is doubled because the check digit will follow it

        for (var i = payload.Length - 1; i >= 0; i--)
        {
            var digit = payload[i] - '0';

            if (doubleIt)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }

            sum += digit;
            doubleIt = !doubleIt;
        }

        return (10 - sum % 10) % 10;
    }

    public static bool IsValid(string? digits)
    {
        if (digits is null || digits.Length < 2 || !digits.All(char.IsAsciiDigit))
            return false;

        return ComputeCheckDigit(digits[..^1]) == digits[^1] - '0';
    }
}
