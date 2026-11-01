namespace MyApp.Application.Abstractions;

/// <summary>The authenticated caller. Only usable inside requests that passed authentication.</summary>
public interface ICurrentUser
{
    Guid UserId { get; }
    bool IsAdmin { get; }
}
