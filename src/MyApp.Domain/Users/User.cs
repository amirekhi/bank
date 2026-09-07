// MyApp.Domain/Users/User.cs
using System;
using MyApp.Domain.Common;

namespace MyApp.Domain.Users;

public class User : Entity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }

    private User() { } // EF Core

    public User(string name, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
    }

    public void ChangeEmail(Email newEmail) => Email = newEmail;

    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.");
        Name = newName;
    }
}