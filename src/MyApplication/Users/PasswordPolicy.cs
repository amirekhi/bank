namespace MyApp.Application.Users;

public static class PasswordPolicy
{
    public const int MinLength = 12;
    public const int MaxLength = 128;

    public static void EnsureValid(string? password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < MinLength)
            throw new ArgumentException($"Password must be at least {MinLength} characters long.");
        if (password.Length > MaxLength)
            throw new ArgumentException($"Password must be at most {MaxLength} characters long.");
    }
}
