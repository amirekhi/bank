namespace MyApp.Domain.Common;

public class InvalidStatusTransitionException : Exception
{
    public InvalidStatusTransitionException(string message) : base(message) { }
}
