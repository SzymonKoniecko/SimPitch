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

    public async Task<IEnumerable<Scoreboard>> GetScoreboardBySimulationIdAsync(Guid simulationId, bool withTeamStats, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT *
            FROM Scoreboard
            WHERE SimulationId = @SimulationId
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var results = await connection.QueryAsync<Scoreboard>(command);

        if (withTeamStats)
            foreach (var scoreboard in results)
                scoreboard.AddTeamRange(await _scoreboardTeamStatsReadRepository.GetScoreboardByScoreboardIdAsync(scoreboard.Id, cancellationToken: cancellationToken));

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
