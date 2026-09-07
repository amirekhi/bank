// MyApp.Domain/Users/Exceptions/UserNotFoundException.cs
using System;

namespace MyApp.Domain.Users.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId)
        : base($"User {userId} was not found.") { }
}