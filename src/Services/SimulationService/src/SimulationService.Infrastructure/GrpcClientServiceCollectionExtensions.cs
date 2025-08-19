using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SportsDataService.League;
using SimPitchProtos.SportsDataService.LeagueRound;
using SimPitchProtos.SportsDataService.MatchRound;

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

        
        return services;
    }
}