using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EngineService.Infrastructure.Middlewares;

public class ProblemDetailsExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProblemDetailsExceptionMiddleware> _logger;

    public ProblemDetailsExceptionMiddleware(RequestDelegate next, ILogger<ProblemDetailsExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);

            var (status, title, type) = MapException(ex);
            var problem = new ProblemDetails
            {
                Title = title,
                Detail = ex.Message,
                Status = status,
                Type = type,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = status ?? (int)HttpStatusCode.InternalServerError;

            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }

    private static (int? status, string title, string type) MapException(Exception ex)
    {
        return ex switch
        {
            // Domenowe: brak zasobu
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found", "https://httpstatuses.io/404"),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found", "https://httpstatuses.io/404"),

            // Walidacja
            ValidationException vex => (StatusCodes.Status400BadRequest, "Validation failed", "https://httpstatuses.io/400"),

            // Niedozwolone/stan
            InvalidOperationException => (StatusCodes.Status409Conflict, "Invalid operation", "https://httpstatuses.io/409"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", "https://httpstatuses.io/401"),

            // Domyślnie 500
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "https://httpstatuses.io/500")
        };
    }
}

// Przykładowe wyjątki domenowe (opcjonalnie)
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public IEnumerable<(string PropertyName, string ErrorMessage)> Errors { get; }

    public ValidationException(string message, IEnumerable<(string PropertyName, string ErrorMessage)> errors = null)
        : base(message)
    {
        Errors = errors ?? Enumerable.Empty<(string, string)>();
    }
}