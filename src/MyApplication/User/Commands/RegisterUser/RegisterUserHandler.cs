using MyApp.Domain.Users;

namespace MyApp.Application.Users.Commands.RegisterUser;

public class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;

    public RegisterUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var existing = await _userRepository.GetByEmail(command.Email, ct);
        if (existing is not null)
            throw new InvalidOperationException("A user with this email already exists.");

        var user = new User(command.Name, new Email(command.Email));
        await _userRepository.Save(user, ct);

        return user.Id;
    }
}
