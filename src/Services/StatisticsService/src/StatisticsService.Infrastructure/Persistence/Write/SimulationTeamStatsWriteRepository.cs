using System;
using Dapper;
using Grpc.Core;
using Newtonsoft.Json;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Infrastructure.Persistence.Write;

public class SimulationTeamStatsWriteRepository : ISimulationTeamStatsWriteRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SimulationTeamStatsWriteRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }

    public async Task CreateSimulationTeamStatsAsync(SimulationTeamStats simulationTeamStats, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = @"
        INSERT INTO SimulationTeamStats
        (
            Id,
            SimulationId,
            TeamId,
            PositionProbbility,
            AverangePoints,
            AverangeWins,
            AverangeLosses,
            AverangeDraws,
            AverangeGoalsFor,
            AverangeGoalsAgainst
        )
        VALUES
        (
            @Id,
            @SimulationId,
            @TeamId,
            @PositionProbbilityJSON,
            @AverangePoints,
            @AverangeWins,
            @AverangeLosses,
            @AverangeDraws,
            @AverangeGoalsFor,
            @AverangeGoalsAgainst
        );";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                simulationTeamStats.Id,
                simulationTeamStats.SimulationId,
                simulationTeamStats.TeamId,
                PositionProbbilityJSON = JsonConvert.SerializeObject(simulationTeamStats.PositionProbbility),
                simulationTeamStats.AverangePoints,
                simulationTeamStats.AverangeWins,
                simulationTeamStats.AverangeLosses,
                simulationTeamStats.AverangeDraws,
                simulationTeamStats.AverangeGoalsFor,
                simulationTeamStats.AverangeGoalsAgainst
            },
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(command);
    }
}
