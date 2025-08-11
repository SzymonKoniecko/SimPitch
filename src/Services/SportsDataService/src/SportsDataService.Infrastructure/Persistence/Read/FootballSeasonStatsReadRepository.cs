using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class FootballSeasonStatsReadRepository : IFootballSeasonStatsReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;

    public FootballSeasonStatsReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }
    public async Task<FootballSeasonStats> GetSeasonStatsByIdAsync(Guid seasonStatsId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM FootballSeasonStats WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = seasonStatsId },
            cancellationToken: cancellationToken

        );

        var stats = await connection.QueryFirstOrDefaultAsync<FootballSeasonStats>(command);

        if (stats == null)
        {
            throw new KeyNotFoundException($"FootballSeasonStats with Id '{seasonStatsId}' was not found.");
        }

        return stats;
    }
    public async Task<IEnumerable<FootballSeasonStats>> GetAllSeasonStatsAsync(CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM FootballSeasonStats ORDER BY Season";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return await connection.QueryAsync<FootballSeasonStats>(command);
    }
}
