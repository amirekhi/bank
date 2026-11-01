using MyApp.Application.Abstractions;
using MyApp.Domain.Users;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password);

public record LoginResult(string AccessToken, DateTime ExpiresAtUtc);

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmail(command.Email, ct);

        if (user is null || !_passwordHasher.Verify(user.PasswordHash, command.Password))
            throw new InvalidCredentialsException();

        // Checked only after the password is proven correct, so status is never leaked to strangers.
        user.EnsureActive();

        var token = _tokenGenerator.Generate(user);
        return new LoginResult(token.Token, token.ExpiresAtUtc);
    }
}
