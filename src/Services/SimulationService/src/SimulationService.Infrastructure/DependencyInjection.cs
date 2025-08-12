using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimulationService.Application.Interfaces;
using SimulationService.Infrastructure.Clients;
namespace SimulationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //  Database
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
    
        //  Clients DI
        services.AddTransient<ILeagueRoundGrpcClient, LeagueRoundGrpcClient>();
        
        //  Read repositories

        //  Write repositories

        return services;
    }
}