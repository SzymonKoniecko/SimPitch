using System;
using Dapper;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Infrastructure.Persistence.Write;

public class ScoreboardTeamStatsWriteRepository : IScoreboardTeamStatsWriteRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ScoreboardTeamStatsWriteRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }
    
    public async Task CreateScoreboardTeamStatsBulkAsync(IEnumerable<ScoreboardTeamStats> teamStatsList, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO ScoreboardTeamStats (Id, ScoreboardId, TeamId, Rank, Points, MatchPlayed, Wins, Losses, Draws, GoalsFor, GoalsAgainst, IsInitialStat)
            VALUES (@Id, @ScoreboardId, @TeamId, @Rank, @Points, @MatchPlayed, @Wins, @Losses, @Draws, @GoalsFor, @GoalsAgainst, @IsInitialStat)";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: teamStatsList.Select(teamStats => new
            {
                teamStats.Id,
                teamStats.ScoreboardId,
                teamStats.TeamId,
                teamStats.Rank,
                teamStats.Points,
                teamStats.MatchPlayed,
                teamStats.Wins,
                teamStats.Losses,
                teamStats.Draws,
                teamStats.GoalsFor,
                teamStats.GoalsAgainst,
                teamStats.IsInitialStat
            }),
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(command);

        return;
    }
}
