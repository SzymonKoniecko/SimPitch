using System;
using Dapper;
using Newtonsoft.Json;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Infrastructure.Persistence.Read;

public class SimulationTeamStatsReadRepository : ISimulationTeamStatsReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SimulationTeamStatsReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }

    public async Task<IEnumerable<SimulationTeamStats>> GetSimulationTeamStatsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT *
            FROM SimulationTeamStats
            WHERE SimulationId = @SimulationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

       var rows = await connection.QueryAsync<SimulationTeamStatsRow>(command);

        var results = rows.Select(row => new SimulationTeamStats(
            row.Id,
            row.SimulationId,
            row.TeamId,
            JsonConvert.DeserializeObject<float[]>(row.PositionProbbility),
            row.AverangePoints,
            row.AverangeWins,
            row.AverangeLosses,
            row.AverangeDraws,
            row.AverangeGoalsFor,
            row.AverangeGoalsAgainst
        ));

        return results;
    }

    public async Task<bool> HasExactNumberOfSimulationTeamStatsAsync(Guid simulationId, int expectedCount, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT COUNT(*) 
            FROM SimulationTeamStats
            WHERE SimulationId = @SimulationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count == expectedCount;
    }
}
