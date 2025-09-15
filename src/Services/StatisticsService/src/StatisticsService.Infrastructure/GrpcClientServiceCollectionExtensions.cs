using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService.SimulationResult;
using SimPitchProtos.SportsDataService.LeagueRound;
using SimPitchProtos.SportsDataService.MatchRound;

namespace StatisticsService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSimulationGrpcClient(this IServiceCollection services, string simulationServiceAddress)
    {

        services.AddGrpcClient<SimulationResultService.SimulationResultServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        });
        
        return services;
    }
    public static IServiceCollection AddSportsDataGrpcClient(this IServiceCollection services, string sportsDataServiceAddress)
    {
        services.AddGrpcClient<LeagueRoundService.LeagueRoundServiceClient>(options =>
        {
            options.Address = new Uri(sportsDataServiceAddress);
        });
        services.AddGrpcClient<MatchRoundService.MatchRoundServiceClient>(options =>
        {
            options.Address = new Uri(sportsDataServiceAddress);
        });
        
        return services;
    }
}
