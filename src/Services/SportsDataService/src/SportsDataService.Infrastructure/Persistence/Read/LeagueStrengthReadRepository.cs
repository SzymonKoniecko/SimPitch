using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class LeagueStrengthReadRepository : ILeagueStrengthReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public LeagueStrengthReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<LeagueStrength>> GetLeagueStrengthsByLeagueIdAsync(Guid leagueId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM LeagueStrength WHERE LeagueId = @leagueId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { leagueId = leagueId },
            cancellationToken: cancellationToken
        );

        var leagueStrengths = await connection.QueryAsync<LeagueStrength>(command);

        if (leagueStrengths == null || leagueStrengths.Count() == 0)
        {
            return new List<LeagueStrength>();
        }

        return leagueStrengths.ToList();
    }
}
