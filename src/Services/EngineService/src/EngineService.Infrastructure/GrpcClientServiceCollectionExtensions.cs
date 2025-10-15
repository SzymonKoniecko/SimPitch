
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService.IterationResult;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimPitchProtos.StatisticsService.Scoreboard;

namespace EngineService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSimulationGrpcClient(this IServiceCollection services, string simulationServiceAddress)
    {
        services.AddGrpcClient<IterationResultService.IterationResultServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        });
        services.AddGrpcClient<SimulationEngineService.SimulationEngineServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        });

        
        return services;
    }
    
    public static IServiceCollection AddStatisticsGrpcClient(this IServiceCollection services, string statisticsServiceAddress)
    {
        services.AddGrpcClient<ScoreboardService.ScoreboardServiceClient>(options =>
        {
            options.Address = new Uri(statisticsServiceAddress);
        });


        return services;
    }
}