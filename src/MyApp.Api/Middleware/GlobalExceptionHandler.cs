using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.BankAccounts.Exceptions;
using MyApp.Domain.Common;
using MyApp.Domain.Users.Exceptions;

namespace MyApp.Api.Middleware;

/// <summary>
/// Single place that turns exceptions into RFC 7807 problem responses, so controllers stay free of try/catch.
/// TODO (Phase 1): introduce a DomainException base class and replace the broad InvalidOperationException mapping.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, title) = Map(exception);

        if (status >= 500)
            _logger.LogError(exception, "Unhandled exception");
        else
            _logger.LogWarning("{ExceptionType}: {Message}", exception.GetType().Name, exception.Message);

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = status >= 500 ? "An unexpected error occurred." : exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json", cancellationToken: ct);
        return true;
    }

    private static (int Status, string Title) Map(Exception exception) => exception switch
    {
        AccountNotFoundException or UserNotFoundException => (StatusCodes.Status404NotFound, "Not found"),
        InvalidCredentialsException or UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
        UserNotActiveException => (StatusCodes.Status403Forbidden, "User is not active"),
        InsufficientFundsException => (StatusCodes.Status422UnprocessableEntity, "Insufficient funds"),

        AccountClosedException or AccountSuspendedException or InvalidStatusTransitionException
            or EmailAlreadyInUseException => (StatusCodes.Status409Conflict, "Conflict"),

        DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "The resource was modified by another request. Please retry."),
        DbUpdateException { InnerException: SqlException { Number: 2601 or 2627 } } => (StatusCodes.Status409Conflict, "Duplicate value"),

        ArgumentException => (StatusCodes.Status400BadRequest, "Invalid request"),
        InvalidOperationException => (StatusCodes.Status409Conflict, "Operation not allowed"),

        _ => (StatusCodes.Status500InternalServerError, "Server error")
    };
}
