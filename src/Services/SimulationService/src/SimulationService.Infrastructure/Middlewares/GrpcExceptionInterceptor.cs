
using System.ComponentModel.DataAnnotations;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
namespace SimulationService.Infrastructure.Middlewares;

public class GrpcExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcExceptionInterceptor> _logger;

    public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
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
            _ => new RpcException(
                new Status(StatusCode.Unknown, "An unexpected error occurred.")
            )
        };
    }
}
