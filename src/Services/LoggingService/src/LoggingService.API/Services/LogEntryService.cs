using Grpc.Core;
using Logging;
using LoggingService.Application.Interfaces;

namespace LoggingService.API.Services;

public class LogEntryService : LogService.LogServiceBase
{
    private readonly ILogRepository _logRepository;

    public LogEntryService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LogEntry(LogEntryRequest request, ServerCallContext context)
    {
        await _logRepository.CreateLogEntryAsync(new Domain.Entities.LogEntry
        {
            Id = Guid.Parse(request.Id),
            Timestamp = request.Timestamp.ToDateTime(),
            Message = request.Message,
            Level = request.Level,
            StackTrace = request.StackTrace,
            Source = request.Source,
            Context = request.Context
        });
        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
