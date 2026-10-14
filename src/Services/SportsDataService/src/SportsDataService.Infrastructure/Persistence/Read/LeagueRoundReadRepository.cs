using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class LeagueRoundReadRepository : ILeagueRoundReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;

    public LeagueRoundReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<LeagueRound>> GetLeagueRoundsBySeasonYearAsync(string seasonYear, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM LeagueRound WHERE SeasonYear = @seasonYear";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { seasonYear = seasonYear },
            cancellationToken: cancellationToken
        );

        // Past seasons are represented only by SeasonStats (no league rounds are seeded for them),
        // so an empty result is a valid answer here - not an error.
        return await connection.QueryAsync<LeagueRound>(command);
    }

    public async Task<IEnumerable<LeagueRound>> GetLeagueRoundsByLeagueIdAsync(Guid leagueId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM LeagueRound WHERE LeagueId = @leagueid";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { leagueid = leagueId },
            cancellationToken: cancellationToken
        );

        var leagueRounds = await connection.QueryAsync<LeagueRound>(command);

        if (leagueRounds == null || leagueRounds.Count() == 0)
        {
            throw new KeyNotFoundException($"LeagueRounds with leagueId '{leagueId}' was not found.");
        }

        return leagueRounds;
    }
}
