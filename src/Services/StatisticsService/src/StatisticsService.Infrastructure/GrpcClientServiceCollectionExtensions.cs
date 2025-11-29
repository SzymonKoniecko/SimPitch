using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService.IterationResult;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimPitchProtos.SportsDataService.LeagueRound;
using SimPitchProtos.SportsDataService.MatchRound;
using SimPitchProtos.SportsDataService.SeasonStats;
using StatisticsService.Application.Consts;

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
            ConnectTimeout = TimeSpan.FromSeconds(10)
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .ConfigureChannel(options =>
        {
            options.HttpHandler = new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(300)
            };
            options.MaxReceiveMessageSize = GrpcConsts.MAX_RECEIVE_MESSAGE_SIZE;
            options.MaxSendMessageSize = GrpcConsts.MAX_SEND_MESSAGE_SIZE;
        });

        services.AddGrpcClient<SimulationEngineService.SimulationEngineServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            ConnectTimeout = TimeSpan.FromSeconds(300)
        })
        .ConfigureChannel(options =>
        {
            options.HttpHandler = new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(300)
            };
            options.MaxReceiveMessageSize = GrpcConsts.MAX_RECEIVE_MESSAGE_SIZE;
            options.MaxSendMessageSize = GrpcConsts.MAX_SEND_MESSAGE_SIZE;
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
        services.AddGrpcClient<SeasonStatsService.SeasonStatsServiceClient>(options =>
        {
            options.Address = new Uri(sportsDataServiceAddress);
        });

        return services;
    }
}
