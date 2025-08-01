using LoggingService.API.Protos;
using Grpc.Core;
using LoggingService.Application.Interfaces;

namespace LoggingService.API.Services;

public class LogEntryService : LogService.LogServiceBase
{
    private readonly ILogRepository _logRepository;

    public LogEntryService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public override Task<Empty> LogEntry(LogEntryRequest request, ServerCallContext context)
    {
        _logRepository.CreateLogEntryAsync(new Domain.Entities.LogEntry
        {
            Id = request.Id,
            Timestamp = DateTime.Parse(request.Timestamp),
            Message = request.Message,
            Level = request.Level,
            StackTrace = request.StackTrace,
            Source = request.Source,
            Context = request.Context
        }).GetAwaiter().GetResult();
        return Task.FromResult(new Empty());
    }
}
