using System;
using Dapper;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Infrastructure.Persistence.Read;

public class ScoreboardReadRepository : IScoreboardReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IScoreboardTeamStatsReadRepository _scoreboardTeamStatsReadRepository;

    public ScoreboardReadRepository(IDbConnectionFactory dbConnectionFactory, IScoreboardTeamStatsReadRepository scoreboardTeamStatsReadRepository)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        _scoreboardTeamStatsReadRepository = scoreboardTeamStatsReadRepository ?? throw new ArgumentNullException(nameof(scoreboardTeamStatsReadRepository));
    }

    public async Task<IEnumerable<Scoreboard>> GetScoreboardByQueryAsync(Guid simulationId, Guid iterationResultId, bool withTeamStats, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        string sql = @"
            SELECT *
            FROM Scoreboard
            WHERE SimulationId = @SimulationId
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        if (iterationResultId != Guid.Empty) // filter for requested iteration result
        {
            sql = @"
                SELECT *
                FROM Scoreboard
                WHERE SimulationId = @SimulationId AND IterationResultId = @IterationResultId
            ";
            command = new CommandDefinition(
                commandText: sql,
                parameters: new { SimulationId = simulationId, IterationResultId = iterationResultId},
                cancellationToken: cancellationToken
            );
        }


        var results = await connection.QueryAsync<Scoreboard>(command);

        if (withTeamStats)
            foreach (var scoreboard in results)
            {
                var scoreboardStats = await _scoreboardTeamStatsReadRepository.GetScoreboardByScoreboardIdAsync(scoreboard.Id, cancellationToken: cancellationToken);
                scoreboard.AddTeamRange(scoreboardStats.Where(x => x.IsInitialStat == false));
                scoreboard.AddTeamRangeInitialStats(scoreboardStats.Where(x => x.IsInitialStat == true));
            }

        return results;
    }

    public async Task<bool> ScoreboardsBySimulationIdExistsAsync(Guid simulationId, int expectedScoreboards, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        string sql = $"SELECT COUNT({expectedScoreboards}) FROM Scoreboard WHERE SimulationId = @SimulationId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }

    public async Task<bool> ScoreboardByIterationResultIdExistsAsync(Guid iterationResultId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = "SELECT COUNT(1) FROM Scoreboard WHERE IterationResultId = @iterationResultId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { IterationResultId = iterationResultId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }
}
