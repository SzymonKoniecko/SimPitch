using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Domain.Services;
using SimulationService.Infrastructure.Clients;
using SimulationService.Infrastructure.Persistence.Read;
using SimulationService.Infrastructure.Persistence.Write;
namespace SimulationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //  Database
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
    
        //  Clients DI
        services.AddTransient<ILeagueRoundGrpcClient, LeagueRoundGrpcClient>();
        services.AddTransient<ILeagueGrpcClient, LeagueGrpcClient>();
        services.AddTransient<IMatchRoundGrpcClient, MatchRoundGrpcClient>();
        services.AddTransient<ISeasonStatsGrpcClient, SeasonStatsGrpcClient>();

        //  Read repositories
        services.AddTransient<IIterationResultReadRepository, IterationResultReadRepository>();
        services.AddTransient<ISimulationOverviewReadRepository, SimulationOverviewReadRepository>();

        //  Write repositories
        services.AddTransient<IIterationResultWriteRepository, IterationResultWriteRepository>();
        services.AddTransient<ISimulationOverviewWriteRepository, SimulationOverviewWriteRepository>();

        // Services
        services.AddTransient<SeasonStatsService>();
        services.AddTransient<MatchSimulatorService>();

        
        return services;
    }
}