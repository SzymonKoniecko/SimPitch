
using System.Threading.Channels;
using EngineService.Application.Consts;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService.IterationResult;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimPitchProtos.StatisticsService.Scoreboard;
using SimPitchProtos.StatisticsService.SimulationStats;

namespace EngineService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSimulationGrpcClient(this IServiceCollection services, string simulationServiceAddress)
    {
        services.AddGrpcClient<IterationResultService.IterationResultServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        }).ConfigureChannel(options =>
        {
            options.MaxReceiveMessageSize = GrpcConsts.MAX_RECEIVE_MESSAGE_SIZE;
            options.MaxSendMessageSize = GrpcConsts.MAX_SEND_MESSAGE_SIZE;
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
        }).ConfigureChannel(options =>
        {
            options.MaxReceiveMessageSize = GrpcConsts.MAX_RECEIVE_MESSAGE_SIZE;
            options.MaxSendMessageSize = GrpcConsts.MAX_SEND_MESSAGE_SIZE;
        });
        services.AddGrpcClient<SimulationStatsService.SimulationStatsServiceClient>(options =>
        {
            options.Address = new Uri(statisticsServiceAddress);
        });

        return services;
    }
}