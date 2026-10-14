using System.ComponentModel.DataAnnotations;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace SimulationService.Infrastructure.Middlewares;

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
        catch (Exception ex) when (context.CancellationToken.IsCancellationRequested)
        {
            throw CancelledByCaller(ex, context);
        }
        catch (RpcException rpcEx)
        {
            // if RpcException exists, log w/o mapping
            _logger.LogWarning(
                rpcEx,
                "gRPC error in request: StatusCode={StatusCode}, Detail={Detail}",
                rpcEx.StatusCode,
                rpcEx.Status.Detail
            );
            throw;
        }
        catch (KeyNotFoundException ex)
        {
            // Missing resource is a client-side (NotFound) result, not a server failure.
            _logger.LogWarning("Resource not found in gRPC request {Method}: {Message}", context.Method, ex.Message);
            throw MapToRpcException(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in gRPC request: {Message}", ex.Message);
            throw MapToRpcException(ex);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex) when (context.CancellationToken.IsCancellationRequested)
        {
            throw CancelledByCaller(ex, context);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in gRPC streaming request: {Message}", ex.Message);
            throw MapToRpcException(ex);
        }
    }

    /// <summary>
    /// The caller cancelled the call (closed the page, stopped the simulation, ...) - this is not a server error.
    /// </summary>
    private RpcException CancelledByCaller(Exception ex, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request {Method} was cancelled by the caller: {Message}", context.Method, ex.Message);
        return new RpcException(new Status(StatusCode.Cancelled, "The call was cancelled by the caller."));
    }

    private RpcException MapToRpcException(Exception ex)
    {
        return ex switch
        {
            FormatException => new RpcException(
                new Status(StatusCode.InvalidArgument, ex.Message)
            ),
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
