using System;
using System.Collections.Concurrent;
using LoggingService.SimPitchProtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SportsDataService.Infrastructure.Logging;

public class GrpcLoggerProvider : ILoggerProvider
{
    private readonly IOptions<GrpcLoggerOptions> _options;
    private readonly LogService.LogServiceClient _grpcClient;
    private readonly ConcurrentDictionary<string, GrpcLogger> _loggers = new();

    public GrpcLoggerProvider(IOptions<GrpcLoggerOptions> options, LogService.LogServiceClient grpcClient)
    {
        _options = options;
        _grpcClient = grpcClient;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new GrpcLogger(name, _options.Value.SourceName, _grpcClient));
    }

    public void Dispose()
    {
        _loggers.Clear();
        GC.SuppressFinalize(this);
    }
}
