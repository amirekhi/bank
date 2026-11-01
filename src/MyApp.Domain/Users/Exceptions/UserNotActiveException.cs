namespace MyApp.Domain.Users.Exceptions;

public class UserNotActiveException : Exception
{
    public UserNotActiveException(Guid userId, UserStatus status)
        : base($"User {userId} is not active (status: {status}).") { }
}
