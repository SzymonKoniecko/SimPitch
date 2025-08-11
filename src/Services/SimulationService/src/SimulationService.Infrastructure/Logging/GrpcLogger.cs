using Google.Protobuf.WellKnownTypes;
using LoggingService.SimPitchProtos;
using Microsoft.Extensions.Logging;
using SimulationService.Infrastructure.Clients;

namespace SimulationService.Infrastructure.Logging;

public class GrpcLogger : ILogger
{
    private readonly IGrpcLoggingClient _grpcClient;
    private readonly string _categoryName;
    private readonly string _sourceName;

    public GrpcLogger(
        string categoryName,
        IGrpcLoggingClient grpcClient,
        string sourceName)
    {
        _categoryName = categoryName;
        _grpcClient = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
        _sourceName = sourceName ?? "SimulationService";
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);

        _ = SendLogAsync(logLevel, message, exception);
    }

    private async Task SendLogAsync(LogLevel logLevel, string message, Exception? exception)
    {
        try
        {
            var request = new LogEntryRequest
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message,
                Level = logLevel.ToString(),
                StackTrace = exception?.StackTrace ?? string.Empty,
                Source = _sourceName,
                Context = _categoryName
            };

            await _grpcClient.LogAsync(request);
        }
        catch
        {
        }
    }
}