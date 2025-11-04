using System;
using Dapper;
using Newtonsoft.Json;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

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

        var rows = await connection.QueryAsync(command);

        var results = rows.Select(row => new SimulationTeamStats
        {
            Id = row.Id,
            SimulationId = row.SimulationId,
            TeamId = row.TeamId,
            PositionProbbility = JsonConvert.DeserializeObject<float[]>(row.PositionProbbility),
            AverangePoints = row.AverangePoints,
            AverangeWins = row.AverangeWins,
            AverangeLosses = row.AverangeLosses,
            AverangeDraws = row.AverangeDraws,
            AverangeGoalsFor = row.AverangeGoalsFor,
            AverangeGoalsAgainst = row.AverangeGoalsAgainst
        });

        return results;
    }
}
