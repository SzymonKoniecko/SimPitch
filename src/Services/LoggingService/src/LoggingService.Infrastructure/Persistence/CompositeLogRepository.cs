using LoggingService.Application.Interfaces;
using LoggingService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LoggingService.Infrastructure.Persistence;

class CompositeLogRepository : ILogRepository
{
    private readonly SqlLogRepository _sqlLogRepository;
    private readonly Elasticsearch.ElasticsearchLogRepository _elasticsearchLogRepository;
    private readonly ILogger<CompositeLogRepository> _logger;

    public CompositeLogRepository(
        SqlLogRepository sqlLogRepository,
        Elasticsearch.ElasticsearchLogRepository elasticsearchLogRepository,
        ILogger<CompositeLogRepository> logger)
    {
        _sqlLogRepository = sqlLogRepository;
        _elasticsearchLogRepository = elasticsearchLogRepository;
        _logger = logger;
    }

    public async Task CreateLogEntryAsync(LogEntry logEntry)
    {
        // MSSQL is the source of truth; Elasticsearch is a best-effort secondary
        // sink for search/analysis, so it must not fail the write path.
        await _sqlLogRepository.CreateLogEntryAsync(logEntry);
        try
        {
            await _elasticsearchLogRepository.CreateLogEntryAsync(logEntry);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to index log entry {LogEntryId} in Elasticsearch", logEntry.Id);
        }
    }
}
