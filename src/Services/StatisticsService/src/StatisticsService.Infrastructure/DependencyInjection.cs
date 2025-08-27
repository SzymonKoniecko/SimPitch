using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Infrastructure.Persistence.Read;
using StatisticsService.Infrastructure.Persistence.Write;

namespace StatisticsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //  Database
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
    
        //  Clients DI

        //  Read repositories
        services.AddScoped<IScoreboardReadRepository, ScoreboardReadRepository>();
        //  Write repositories
        services.AddScoped<IScoreboardWriteRepository, ScoreboardWriteRepository>();

        // Services

        return services;
    }
}