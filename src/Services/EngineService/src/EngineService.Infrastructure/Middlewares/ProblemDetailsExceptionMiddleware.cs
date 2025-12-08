using System.Net;
using System.Text.Json;
using Grpc.Core;
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
            // gRPC Exceptions
            RpcException rpcEx => MapRpcException(rpcEx),


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


    private static (int? status, string title, string type) MapRpcException(RpcException rpcEx)
    {
        return rpcEx.StatusCode switch
        {
            StatusCode.NotFound => (StatusCodes.Status404NotFound, "Resource not found", "https://httpstatuses.io/404"),
            StatusCode.InvalidArgument => (StatusCodes.Status400BadRequest, "Invalid argument", "https://httpstatuses.io/400"),
            StatusCode.PermissionDenied => (StatusCodes.Status403Forbidden, "Permission denied", "https://httpstatuses.io/403"),
            StatusCode.Unauthenticated => (StatusCodes.Status401Unauthorized, "Unauthenticated", "https://httpstatuses.io/401"),
            StatusCode.AlreadyExists => (StatusCodes.Status409Conflict, "Already exists", "https://httpstatuses.io/409"),
            StatusCode.FailedPrecondition => (StatusCodes.Status412PreconditionFailed, "Failed precondition", "https://httpstatuses.io/412"),
            StatusCode.OutOfRange => (StatusCodes.Status400BadRequest, "Out of range", "https://httpstatuses.io/400"),
            StatusCode.Unimplemented => (StatusCodes.Status501NotImplemented, "Not implemented", "https://httpstatuses.io/501"),
            StatusCode.Internal => (StatusCodes.Status500InternalServerError, "Internal error", "https://httpstatuses.io/500"),
            StatusCode.Unavailable => (StatusCodes.Status503ServiceUnavailable, "Service unavailable", "https://httpstatuses.io/503"),
            StatusCode.DeadlineExceeded => (StatusCodes.Status504GatewayTimeout, "Deadline exceeded", "https://httpstatuses.io/504"),
            _ => (StatusCodes.Status500InternalServerError, "Unknown gRPC error", "https://httpstatuses.io/500")
        };
    }
}


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
