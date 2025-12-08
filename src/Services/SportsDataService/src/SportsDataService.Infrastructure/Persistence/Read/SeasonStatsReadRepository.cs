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
    public async Task<IEnumerable<SeasonStats>> GetSeasonsStatsByTeamIdAsync(Guid teamId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM SeasonStats WHERE TeamId = @teamId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { TeamId = teamId },
            cancellationToken: cancellationToken

        );

        var stats = await connection.QueryAsync<SeasonStats>(command);

        if (stats == null || !stats.Any())
        {
            return null;
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
