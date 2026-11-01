namespace MyApp.Domain.Common;

/// <summary>
/// Base class for every business-rule violation. <see cref="Code"/> is the stable error code
/// from the error catalogue (section 14 of the specification); the API maps it to an HTTP status.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string code, string message) : base(message) => Code = code;

    public string Code { get; }
}

/// <summary>Input is malformed or out of range (VALIDATION_FAILED).</summary>
public sealed class ValidationFailedException : DomainException
{
    public ValidationFailedException(string message) : base("VALIDATION_FAILED", message) { }
}
