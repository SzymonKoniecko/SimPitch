using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class MatchRoundReadRepository : IMatchRoundReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;

    public MatchRoundReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<MatchRound>> GetMatchRoundsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM MatchRound WHERE RoundId = @roundId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { RoundId = roundId }
        );

        var MatchRounds = await connection.QueryAsync<MatchRound>(command);

        if (MatchRounds == null)
        {
            throw new KeyNotFoundException($"MatchRounds with roundId '{roundId}' was not found.");
        }

        return MatchRounds;
    }
}
