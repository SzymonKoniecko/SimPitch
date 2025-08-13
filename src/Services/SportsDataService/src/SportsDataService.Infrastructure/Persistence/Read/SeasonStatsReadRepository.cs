using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class SeasonStatsReadRepository : ISeasonStatsReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;

    public SeasonStatsReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }
    public async Task<SeasonStats> GetSeasonStatsByIdAsync(Guid seasonStatsId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM SeasonStats WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = seasonStatsId },
            cancellationToken: cancellationToken

        );

        var stats = await connection.QueryFirstOrDefaultAsync<SeasonStats>(command);

        if (stats == null)
        {
            throw new KeyNotFoundException($"SeasonStats with Id '{seasonStatsId}' was not found.");
        }

        return stats;
    }
    public async Task<IEnumerable<SeasonStats>> GetAllSeasonStatsAsync(CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM SeasonStats ORDER BY Season";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return await connection.QueryAsync<SeasonStats>(command);
    }
}
