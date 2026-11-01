using MyApp.Application.Abstractions;
using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.Users.Queries.GetCurrentUser;

public record CurrentUserDto(Guid Id, string Name, string Email, string Role, string Status);

public class GetCurrentUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public GetCurrentUserHandler(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<CurrentUserDto> Handle(CancellationToken ct)
    {
        var id = _currentUser.UserId;
        var user = await _userRepository.GetById(id, ct) ?? throw new UserNotFoundException(id);

        return new CurrentUserDto(user.Id, user.Name, user.Email.Value, user.Role.ToString(), user.Status.ToString());
    }
}
