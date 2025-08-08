using Microsoft.Extensions.Logging;
using SportsDataService.Infrastructure.Clients;

namespace SportsDataService.Infrastructure.Logging;
[ProviderAlias("GrpcLogger")]
public class GrpcLoggerProvider : ILoggerProvider
{
    private readonly IGrpcLoggingClient _grpcLoggingClient;
    private readonly string _sourceName;

    public GrpcLoggerProvider(IGrpcLoggingClient grpcLoggingClient, string sourceName)
    {
        _grpcLoggingClient = grpcLoggingClient ?? throw new ArgumentNullException(nameof(grpcLoggingClient));
        _sourceName = sourceName ?? "SportsDataService";
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new GrpcLogger(categoryName, _grpcLoggingClient, _sourceName);
    }

    public void Dispose()
    {
    }
}
