using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SportsDataService.LeagueRound;
using SimPitchProtos.SportsDataService.MatchRound;

namespace SimulationService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSportsDataGrpcClient(this IServiceCollection services)
    {
        var sportsDataServiceAddress = Environment.GetEnvironmentVariable("SportsDataService__Address");
        if (string.IsNullOrEmpty(sportsDataServiceAddress))
        {
            throw new InvalidOperationException("Environment variable 'SportsDataService__Address' is not set.");
        }

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