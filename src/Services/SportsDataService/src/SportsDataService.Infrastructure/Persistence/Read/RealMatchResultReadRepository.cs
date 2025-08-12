using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class RealMatchResultReadRepository : IRealMatchResultReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;

    public RealMatchResultReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<RealMatchResult>> GetRealMatchResultsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM RealMatchResult WHERE RoundId = @roundId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { RoundId = roundId }
        );

        var realMatchResults = await connection.ExecuteScalarAsync<List<RealMatchResult>>(command);

        if (realMatchResults == null)
        {
            throw new KeyNotFoundException($"RealMatchResults with roundId '{roundId}' was not found.");
        }

        return realMatchResults;
    }
}
