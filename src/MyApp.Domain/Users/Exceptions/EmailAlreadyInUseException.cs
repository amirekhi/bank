namespace MyApp.Domain.Users.Exceptions;

public class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException(string email)
        : base($"A user with email {email} already exists.") { }
}
