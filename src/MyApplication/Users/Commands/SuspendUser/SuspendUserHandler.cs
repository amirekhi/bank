using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.Users.Commands.SuspendUser;

public record SuspendUserCommand(Guid UserId);

public class SuspendUserHandler
{
    private readonly IUserRepository _userRepository;

    public SuspendUserHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task Handle(SuspendUserCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetById(command.UserId, ct)
            ?? throw new UserNotFoundException(command.UserId);

        user.Suspend();
        await _userRepository.Save(user, ct);
    }
}
