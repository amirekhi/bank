using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.Users.Commands.ReinstateUser;

public record ReinstateUserCommand(Guid UserId);

public class ReinstateUserHandler
{
    private readonly IUserRepository _userRepository;

    public ReinstateUserHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task Handle(ReinstateUserCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetById(command.UserId, ct)
            ?? throw new UserNotFoundException(command.UserId);

        user.Reinstate();
        await _userRepository.Save(user, ct);
    }
}
