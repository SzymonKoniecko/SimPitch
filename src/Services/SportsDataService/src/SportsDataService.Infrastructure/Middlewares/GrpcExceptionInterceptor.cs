using System.ComponentModel.DataAnnotations;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace SportsDataService.Infrastructure.Middlewares;

public class GrpcExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcExceptionInterceptor> _logger;
    private readonly IHostEnvironment _env;

    public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger, IHostEnvironment env)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _env = env ?? throw new ArgumentNullException(nameof(env));
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation(
                "GRPC client: method {Method} executed",
                context.Method
            );
        }

        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in gRPC request: {Message}", ex.Message);
            throw MapToRpcException(ex);
        }
    }

    private RpcException MapToRpcException(Exception ex)
    {
        return ex switch
        {
            KeyNotFoundException => new RpcException(
                new Status(StatusCode.NotFound, ex.Message)
            ),
            ValidationException => new RpcException(
                new Status(StatusCode.InvalidArgument, ex.Message)
            ),
            ArgumentException => new RpcException(
                new Status(StatusCode.InvalidArgument, ex.Message)
            ),
            UnauthorizedAccessException => new RpcException(
                new Status(StatusCode.PermissionDenied, ex.Message)
            ),
            NotSupportedException => new RpcException(
                new Status(StatusCode.Unimplemented, ex.Message)
            ),
            TimeoutException => new RpcException(
                new Status(StatusCode.DeadlineExceeded, ex.Message)
            ),
            InvalidOperationException => new RpcException(
                new Status(StatusCode.FailedPrecondition, ex.Message)
            ),
            IOException => new RpcException(
                new Status(StatusCode.Unavailable, ex.Message)
            ),
            _ => new RpcException(
                new Status(StatusCode.Unknown, "An unexpected error occurred.")
            )
        };
    }
}
