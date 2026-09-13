using Elastic.Clients.Elasticsearch;
using LoggingService.Application.Interfaces;
using LoggingService.Infrastructure.Elasticsearch;
using LoggingService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace LoggingService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<SqlLogRepository>();

        services.Configure<ElasticsearchOptions>(configuration.GetSection(ElasticsearchOptions.SectionName));
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ElasticsearchOptions>>().Value;
            var settings = new ElasticsearchClientSettings(new Uri(options.Uri));
            return new ElasticsearchClient(settings);
        });
        services.AddScoped<ElasticsearchLogRepository>();

        services.AddScoped<ILogRepository, CompositeLogRepository>();
        return services;
    }
}
