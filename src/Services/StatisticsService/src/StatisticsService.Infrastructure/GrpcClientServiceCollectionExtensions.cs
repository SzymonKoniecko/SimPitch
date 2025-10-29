using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService.IterationResult;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimPitchProtos.SportsDataService.LeagueRound;
using SimPitchProtos.SportsDataService.MatchRound;

namespace StatisticsService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSimulationGrpcClient(this IServiceCollection services, string simulationServiceAddress)
    {
        services.AddGrpcClient<IterationResultService.IterationResultServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            ConnectTimeout = TimeSpan.FromSeconds(10) // timeout na połączenie TCP
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5)) // opcjonalne, kontroluje lifetime HttpHandlera
        .ConfigureChannel(options =>
        {
            options.HttpHandler = new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(300)
            };
        });

        services.AddGrpcClient<SimulationEngineService.SimulationEngineServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            ConnectTimeout = TimeSpan.FromSeconds(300)
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
