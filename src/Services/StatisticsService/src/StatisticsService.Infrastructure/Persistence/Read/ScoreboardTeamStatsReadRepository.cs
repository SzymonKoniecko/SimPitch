using System;
using Dapper;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Infrastructure.Persistence.Read;

public class ScoreboardTeamStatsReadRepository : IScoreboardTeamStatsReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public ScoreboardTeamStatsReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }
    public async Task<IEnumerable<ScoreboardTeamStats>> GetScoreboardByScoreboardIdAsync(Guid scoreboardId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        const string sql = @"
            SELECT *
            FROM ScoreboardTeamStats
            WHERE ScoreboardId = @ScoreboardId
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { ScoreboardId = scoreboardId },
            cancellationToken: cancellationToken
        );

        var results = await connection.QueryAsync<ScoreboardTeamStats>(command);
        return results;
    }
}
