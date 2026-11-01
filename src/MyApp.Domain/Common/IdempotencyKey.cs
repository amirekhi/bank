namespace MyApp.Domain.Common;

/// <summary>A client-chosen token that makes a command safe to retry (S-01, P-21): 8 to 64 safe characters.</summary>
public sealed record IdempotencyKey
{
    public const int MinLength = 8;
    public const int MaxLength = 64;

    private IdempotencyKey(string value) => Value = value;

    public string Value { get; }

    public static IdempotencyKey Parse(string? value)
    {
        if (value is null || value.Length < MinLength || value.Length > MaxLength)
            throw new ValidationFailedException(
                $"The idempotency key must be {MinLength} to {MaxLength} characters long.");

        if (!value.All(IsAllowed))
            throw new ValidationFailedException(
                "The idempotency key may contain only letters, digits, '-', '_', '.' and ':'.");

        return new IdempotencyKey(value);
    }

    private static bool IsAllowed(char c)
        => char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.' or ':';

    public override string ToString() => Value;
}
