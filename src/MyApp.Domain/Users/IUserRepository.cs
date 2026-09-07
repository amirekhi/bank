// MyApp.Domain/Users/IUserRepository.cs
namespace MyApp.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetById(Guid id, CancellationToken ct);
    Task<User?> GetByEmail(string email, CancellationToken ct);
    Task Save(User user, CancellationToken ct);
}