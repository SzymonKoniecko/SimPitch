
using Microsoft.Extensions.DependencyInjection;

namespace EngineService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSportsDataGrpcClient(this IServiceCollection services, string sportsDataServiceAddress)
    {

        // services.AddGrpcClient<LeagueRoundService.LeagueRoundServiceClient>(options =>
        // {
        //     options.Address = new Uri(sportsDataServiceAddress);
        // });
        // services.AddGrpcClient<LeagueService.LeagueServiceClient>(options =>
        // {
        //     options.Address = new Uri(sportsDataServiceAddress);
        // });
        // services.AddGrpcClient<MatchRoundService.MatchRoundServiceClient>(options =>
        // {
        //     options.Address = new Uri(sportsDataServiceAddress);
        // });
        // services.AddGrpcClient<SeasonStatsService.SeasonStatsServiceClient>(options =>
        // {
        //     options.Address = new Uri(sportsDataServiceAddress);
        // });

        
        return services;
    }
}