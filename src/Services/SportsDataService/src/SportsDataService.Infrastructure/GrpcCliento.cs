using System;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using LoggingService.SimPitchProtos;
namespace SportsDataService.Infrastructure;

public class GrpcCliento
{
    private readonly LogService.LogServiceClient _grpcClient;
    public GrpcCliento(string address)
    {
        var channel = GrpcChannel.ForAddress(address);

        // Initialize the client with the channel
        _grpcClient = new LogService.LogServiceClient(channel);
    }
    public async Task LogAsync()
    {
        var request = new LogEntryRequest
        {
            Id = Guid.NewGuid().ToString(),
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            Message = "message",
            Level = "logLevel.ToString()",
            StackTrace = "exception?.StackTrace ?? string.Empty,",
            Source = "_sourceName", // Nazwa mikroserwisu, np. "SportsDataService"
            Context = "_categoryName" // Kontekst logu, np. nazwa klasy
        };

        try
        {
            await _grpcClient.LogEntryAsync(request);
        }
        catch (Exception ex)
        {
            // Jeśli usługa logowania jest niedostępna, zaloguj błąd do konsoli
            // aby uniknąć pętli logowania i awarii aplikacji.
            Console.WriteLine($"[CRITICAL] Could not send log to gRPC LoggingService: {ex.Message}");
        }
    }
}
