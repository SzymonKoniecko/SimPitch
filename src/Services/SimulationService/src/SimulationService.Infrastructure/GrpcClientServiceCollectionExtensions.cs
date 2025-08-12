using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SportsDataService.LeagueRound;

namespace SimulationService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSportsDataGrpcClient(this IServiceCollection services)
    {
        var address = Environment.GetEnvironmentVariable("SportsDataService__Address");
        if (string.IsNullOrEmpty(address))
        {
            throw new InvalidOperationException("Environment variable 'SportsDataService__Address' is not set.");
        }

        services.AddGrpcClient<LeagueRoundService.LeagueRoundServiceClient>(options =>
        {
            options.Address = new Uri(address);
        });

        return services;
    }
}