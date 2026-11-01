namespace MyApp.Domain.Users.Exceptions;

public class InvalidCredentialsException : Exception
{
    // Deliberately vague: never reveal whether the email or the password was wrong.
    public InvalidCredentialsException() : base("Invalid email or password.") { }
}
