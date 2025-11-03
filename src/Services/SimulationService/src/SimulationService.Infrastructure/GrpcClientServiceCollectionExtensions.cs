using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SportsDataService.League;
using SimPitchProtos.SportsDataService.LeagueRound;
using SimPitchProtos.SportsDataService.MatchRound;
using SimPitchProtos.SportsDataService.SeasonStats;
using SimPitchProtos.StatisticsService.Scoreboard;
using SimulationService.Domain.Services;
using SeasonStatsService = SimPitchProtos.SportsDataService.SeasonStats.SeasonStatsService;

namespace SimulationService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSportsDataGrpcClient(this IServiceCollection services, string sportsDataServiceAddress)
    {

        services.AddGrpcClient<LeagueRoundService.LeagueRoundServiceClient>(options =>
        {
            options.Address = new Uri(sportsDataServiceAddress);
        });
        services.AddGrpcClient<LeagueService.LeagueServiceClient>(options =>
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
    
    public static IServiceCollection AddStatisticsGrpcClient(this IServiceCollection services, string statisticsServiceAddress)
    {

        services.AddGrpcClient<ScoreboardService.ScoreboardServiceClient>(options =>
        {
            options.Address = new Uri(statisticsServiceAddress);
        });
        
        return services;
    }
}