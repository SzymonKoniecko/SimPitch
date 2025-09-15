using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Interfaces;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;
using StatisticsService.Infrastructure.Clients;
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
        services.AddTransient<ISimulationResultGrpcClient, SimulationResultGrpcClient>();
        services.AddTransient<ILeagueRoundGrpcClient, LeagueRoundGrpcClient>();
        services.AddTransient<IMatchRoundGrpcClient, MatchRoundGrpcClient>();

        //  Read repositories
        services.AddScoped<IScoreboardReadRepository, ScoreboardReadRepository>();
        services.AddScoped<IScoreboardTeamStatsReadRepository, ScoreboardTeamStatsReadRepository>();
        //  Write repositories
        services.AddScoped<IScoreboardWriteRepository, ScoreboardWriteRepository>();
        services.AddScoped<IScoreboardTeamStatsWriteRepository, ScoreboardTeamStatsWriteRepository>();

        // Services
        services.AddTransient<ScoreboardService>();
        services.AddTransient<ScoreboardTeamStatsService>();

        return services;
    }
}