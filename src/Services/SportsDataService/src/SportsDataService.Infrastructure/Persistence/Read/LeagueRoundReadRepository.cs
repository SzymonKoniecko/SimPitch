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

    public async Task<List<LeagueRound>> GetLeagueRoundsBySeasonYearAsync(string seasonYear, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM LeagueRound WHERE SeasonYear = @seasonYear";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SeasonYear = seasonYear }
        );

        var leagueRounds = await connection.QueryAsync<IEnumerable<LeagueRound>>(command);

        if (leagueRounds == null || leagueRounds.Count() == 0)
        {
            throw new KeyNotFoundException($"LeagueRounds with seasonYear '{seasonYear}' was not found.");
        }

        return (List<LeagueRound>)leagueRounds;
    }
}
