using LoggingService.Domain.Entities;

namespace LoggingService.Application.Interfaces;
public interface ILogRepository
{
    Task CreateLogEntryAsync(LogEntry logEntry);
}