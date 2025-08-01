using Dapper;
using LoggingService.Application.Interfaces;
using LoggingService.Domain.Entities;

namespace LoggingService.Infrastructure.Persistence;
class LogRepository : ILogRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public LogRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task CreateLogEntryAsync(LogEntry logEntry)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = "INSERT INTO LogEntries (Id, Timestamp, Message, Level, StackTrace, Source, Context) " +
                           "VALUES (@Id, @Timestamp, @Message, @Level, @StackTrace, @Source, @Context)";
        await connection.ExecuteAsync(sql, logEntry);
    }
}