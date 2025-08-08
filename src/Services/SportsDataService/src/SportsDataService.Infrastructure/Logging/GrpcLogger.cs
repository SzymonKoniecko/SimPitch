using System;
using Google.Protobuf.WellKnownTypes;
using LoggingService.SimPitchProtos;
using Microsoft.Extensions.Logging;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Infrastructure.Logging;

public class GrpcLogger : ILogger
{
    private readonly string _categoryName;
    private readonly string _sourceName;
    private readonly LogService.LogServiceClient _grpcClient;

    public GrpcLogger(string categoryName, string sourceName, LogService.LogServiceClient grpcClient)
    {
        _categoryName = categoryName;
        _sourceName = sourceName;
        _grpcClient = grpcClient;
    }

    // Zakresy (scopes) można zaimplementować, jeśli są potrzebne, w przeciwnym razie prosta implementacja
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        var request = new LogEntryRequest
        {
            Id = Guid.NewGuid().ToString(),
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            Message = message,
            Level = logLevel.ToString(),
            StackTrace = exception?.StackTrace ?? string.Empty,
            Source = _sourceName, // Nazwa mikroserwisu, np. "SportsDataService"
            Context = _categoryName // Kontekst logu, np. nazwa klasy
        };

        // Wysyłamy log asynchronicznie w tle (fire-and-forget)
        // aby nie blokować wykonania aplikacji.
        // Ważne: W prawdziwej aplikacji warto dodać mechanizm kolejkowania i ponawiania prób.
        _ = Task.Run(async () =>
        {
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
        });
    }
}
