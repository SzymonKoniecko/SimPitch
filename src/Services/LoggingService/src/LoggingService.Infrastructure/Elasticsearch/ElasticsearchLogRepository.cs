using Elastic.Clients.Elasticsearch;
using LoggingService.Application.Interfaces;
using LoggingService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace LoggingService.Infrastructure.Elasticsearch;

class ElasticsearchLogRepository : ILogRepository
{
    private readonly ElasticsearchClient _client;
    private readonly string _indexFormat;

    public ElasticsearchLogRepository(ElasticsearchClient client, IOptions<ElasticsearchOptions> options)
    {
        _client = client;
        _indexFormat = options.Value.IndexFormat;
    }

    public async Task CreateLogEntryAsync(LogEntry logEntry)
    {
        var index = string.Format(_indexFormat, logEntry.Timestamp);
        await _client.IndexAsync(logEntry, index, logEntry.Id.ToString());
    }
}
