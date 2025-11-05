using EngineService.Application;
using EngineService.Application.Interfaces;
using EngineService.Infrastructure.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace EngineService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //  Database
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

        //  Clients DI
        services.AddTransient<IScoreboardGrpcClient, ScoreboardGrpcClient>();
        services.AddTransient<IIterationResultGrpcClient, IterationResultGrpcClient>();
        services.AddTransient<ISimulationEngineGrpcClient, SimulationEngineGrpcClient>();
        services.AddTransient<ISimulationStatsGrpcClient, SimulationStatsGrpcClient>();

        //  Read repositories

        //  Write repositories

        // Services

        return services;
    }
}