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

    public async Task<IEnumerable<MatchRound>> GetMatchRoundsAsync(CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM MatchRound";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        var MatchRounds = await connection.QueryAsync<MatchRound>(command);

        if (MatchRounds == null)
        {
            throw new KeyNotFoundException($"There is no match rounds - please execute scripts");
        }

        return MatchRounds;
    }
}
