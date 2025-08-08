using Grpc.Net.Client;
using LoggingService.SimPitchProtos;
using Microsoft.Extensions.Configuration;

namespace SportsDataService.Infrastructure.Clients;

public interface IGrpcLoggingClient
{
    Task LogAsync(LogEntryRequest request);
}
public class GrpcLoggingClient : IGrpcLoggingClient
{
    private readonly LogService.LogServiceClient _grpcClient;

    public GrpcLoggingClient(IConfiguration configuration)
    {
        var address = Environment.GetEnvironmentVariable("GrpcLogging__Address");
        var addressFromConfig = configuration["GrpcLogging:Address"];
        if (string.IsNullOrWhiteSpace(address) && string.IsNullOrWhiteSpace(addressFromConfig))
        {
            throw new InvalidOperationException(
                "Brak konfiguracji gRPC Logging — ustaw zmienną środowiskową 'GrpcLogging__Address' lub 'GrpcLogging:Address'."
            );
        }

        var channel = GrpcChannel.ForAddress(address);
        _grpcClient = new LogService.LogServiceClient(channel);
    }

    public async Task LogAsync(LogEntryRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        await _grpcClient.LogEntryAsync(request);
    }
}