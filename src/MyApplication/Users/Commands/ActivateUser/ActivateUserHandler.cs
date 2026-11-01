using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.Users.Commands.ActivateUser;

public record ActivateUserCommand(Guid UserId);

public class ActivateUserHandler
{
    private readonly IUserRepository _userRepository;

    public ActivateUserHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task Handle(ActivateUserCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetById(command.UserId, ct)
            ?? throw new UserNotFoundException(command.UserId);

        user.Activate();
        await _userRepository.Save(user, ct);
    }
}
