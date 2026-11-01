using MyApp.Domain.Common;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Domain.Users;

public class User : Entity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }

    private User() { } // EF Core

    private User(string name, Email email, string passwordHash, UserRole role, UserStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.");

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        Status = status;
    }

    /// <summary>Self-registration: a customer who must be activated by an admin before using the bank.</summary>
    public static User Register(string name, Email email, string passwordHash)
        => new(name, email, passwordHash, UserRole.Customer, UserStatus.PendingActivation);

    /// <summary>Admins are created by the system (seeding), never through public registration.</summary>
    public static User CreateAdmin(string name, Email email, string passwordHash)
        => new(name, email, passwordHash, UserRole.Admin, UserStatus.Active);

    public void Activate()
    {
        if (Status != UserStatus.PendingActivation)
            throw new InvalidStatusTransitionException($"User {Id} cannot be activated from status {Status}.");
        Status = UserStatus.Active;
    }

    public void Suspend()
    {
        if (Role == UserRole.Admin)
            throw new InvalidStatusTransitionException("Admin users cannot be suspended.");
        if (Status != UserStatus.Active)
            throw new InvalidStatusTransitionException($"User {Id} cannot be suspended from status {Status}.");
        Status = UserStatus.Suspended;
    }

    public void Reinstate()
    {
        if (Status != UserStatus.Suspended)
            throw new InvalidStatusTransitionException($"User {Id} cannot be reinstated from status {Status}.");
        Status = UserStatus.Active;
    }

    public void EnsureActive()
    {
        if (Status != UserStatus.Active)
            throw new UserNotActiveException(Id, Status);
    }

    public void ChangeEmail(Email newEmail) => Email = newEmail;

    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.");
        Name = newName;
    }
}
