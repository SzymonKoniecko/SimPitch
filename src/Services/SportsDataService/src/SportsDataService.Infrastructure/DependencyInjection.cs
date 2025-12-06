using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Domain.Interfaces.Write;
using SportsDataService.Infrastructure.Persistence.Read;
using SportsDataService.Infrastructure.Persistence.Teams;
using SportsDataService.Infrastructure.Persistence.Write;

namespace SportsDataService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

        // Read repositories
        services.AddTransient<ICountryReadRepository, CountryReadRepository>();
        services.AddTransient<ISeasonStatsReadRepository, SeasonStatsReadRepository>();
        services.AddTransient<ILeagueReadRepository, LeagueReadRepository>();
        services.AddTransient<ILeagueStrengthReadRepository, LeagueStrengthReadRepository>();
        services.AddTransient<IStadiumReadRepository, StadiumReadRepository>();
        services.AddTransient<ITeamReadRepository, TeamReadRepository>();
        services.AddTransient<IMatchRoundReadRepository, MatchRoundReadRepository>();
        services.AddTransient<ILeagueRoundReadRepository, LeagueRoundReadRepository>();
        services.AddTransient<ICompetitionMembershipReadRepository, CompetitionMembershipReadRepository>();

        // Write repositories
        services.AddTransient<ITeamWriteRepository, TeamWriteRepository>();
        services.AddTransient<IStadiumWriteRepository, StadiumWriteRepository>();
        
        return services;
    }
}