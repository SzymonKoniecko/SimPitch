namespace LoggingService.Infrastructure.Elasticsearch;

public class ElasticsearchOptions
{
    public const string SectionName = "Elasticsearch";

    public string Uri { get; set; } = "http://elasticsearch:9200";
    public string IndexFormat { get; set; } = "simpitch-logs-{0:yyyy.MM.dd}";
}
