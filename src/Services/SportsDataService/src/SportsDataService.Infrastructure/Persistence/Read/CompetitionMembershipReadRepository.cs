using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class CompetitionMembershipReadRepository : ICompetitionMembershipReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;
    public CompetitionMembershipReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<CompetitionMembership>> GetAllByTeamIdAsync(Guid teamId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM CompetitionMembership WHERE TeamId = @TeamId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { TeamId = teamId },
            cancellationToken: cancellationToken
        );

        var competitionMemberships = await connection.QueryAsync<CompetitionMembership>(command);

        if (competitionMemberships == null)
        {
            throw new KeyNotFoundException($"CompetitionMembership with TeamId '{teamId}' was not found.");
        }

        return competitionMemberships;
    }

    public async Task<IEnumerable<CompetitionMembership>> GetAllByLeagueIdAndSeasonYearAsync(Guid leagueId, string seasonYear, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM CompetitionMembership WHERE LeagueId = @LeagueId AND SeasonYear = @SeasonYear";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { LeagueId = leagueId, SeasonYear = seasonYear },
            cancellationToken: cancellationToken
        );

        var competitionMemberships = await connection.QueryAsync<CompetitionMembership>(command);

        if (competitionMemberships == null)
        {
            throw new KeyNotFoundException($"CompetitionMembership with leagueId '{leagueId}' and season year: {seasonYear} was not found.");
        }

        return competitionMemberships;
    }
}
