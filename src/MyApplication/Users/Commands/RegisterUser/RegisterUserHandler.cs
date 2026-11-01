using MyApp.Application.Abstractions;
using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.Users.Commands.RegisterUser;

public class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        PasswordPolicy.EnsureValid(command.Password);
        var email = new Email(command.Email);

        if (await _userRepository.GetByEmail(email.Value, ct) is not null)
            throw new EmailAlreadyInUseException(email.Value);

        // New customers start as PendingActivation; an admin must activate them.
        var user = User.Register(command.Name, email, _passwordHasher.Hash(command.Password));
        await _userRepository.Save(user, ct);

        return user.Id;
    }
}
