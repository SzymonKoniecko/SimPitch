using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Features.Scoreboards.Services;
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
        services.AddTransient<IIterationResultGrpcClient, IterationResultGrpcClient>();
        services.AddTransient<ILeagueRoundGrpcClient, LeagueRoundGrpcClient>();
        services.AddTransient<IMatchRoundGrpcClient, MatchRoundGrpcClient>();
        services.AddTransient<ISimulationEngineGrpcClient, SimulationEngineGrpcClient>();

        //  Read repositories
        services.AddScoped<IScoreboardReadRepository, ScoreboardReadRepository>();
        services.AddScoped<IScoreboardTeamStatsReadRepository, ScoreboardTeamStatsReadRepository>();
        services.AddScoped<ISimulationTeamStatsReadRepository, SimulationTeamStatsReadRepository>();
        //  Write repositories
        services.AddScoped<IScoreboardWriteRepository, ScoreboardWriteRepository>();
        services.AddScoped<IScoreboardTeamStatsWriteRepository, ScoreboardTeamStatsWriteRepository>();
        services.AddScoped<ISimulationTeamStatsWriteRepository, SimulationTeamStatsWriteRepository>();

        // Services
        services.AddTransient<ScoreboardService>();
        services.AddTransient<ScoreboardTeamStatsService>();
        services.AddTransient<IScoreboardDataService, ScoreboardDataService>();

        return services;
    }
}