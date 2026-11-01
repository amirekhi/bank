namespace MyApp.Domain.Common;

/// <summary>Free text an Admin must give for sensitive actions: 10 to 500 characters after trimming (P-22).</summary>
public sealed record Reason
{
    public const int MinLength = 10;
    public const int MaxLength = 500;

    private Reason(string value) => Value = value;

    public string Value { get; }

    public static Reason Parse(string? text)
    {
        var trimmed = text?.Trim();

        if (string.IsNullOrEmpty(trimmed) || trimmed.Length < MinLength || trimmed.Length > MaxLength)
            throw new ValidationFailedException(
                $"A reason must be {MinLength} to {MaxLength} characters long.");

        return new Reason(trimmed);
    }

    public override string ToString() => Value;
}
