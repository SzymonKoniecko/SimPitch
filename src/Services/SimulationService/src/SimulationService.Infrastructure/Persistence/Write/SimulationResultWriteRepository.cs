using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Infrastructure.Persistence.Write;

public class SimulationResultWriteRepository : ISimulationResultWriteRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public SimulationResultWriteRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task CreateSimulationResultAsync(SimulationResult simulationResult, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO SimulationResult 
            (Id, SimulationId, SimulationIndex, StartDate, ExecutionTime, SimulatedMatchRounds, LeagueStrength, PriorLeagueStrength, SimulationParams, Raport)
            VALUES 
            (@Id, @SimulationId, @SimulationIndex, @StartDate, @ExecutionTime, @SimulatedMatchRounds, @LeagueStrength, @PriorLeagueStrength, @SimulationParams, @Raport);
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                simulationResult.Id,
                simulationResult.SimulationId,
                simulationResult.SimulationIndex,
                simulationResult.StartDate,
                simulationResult.ExecutionTime,
                simulationResult.SimulatedMatchRounds,
                simulationResult.LeagueStrength,
                simulationResult.PriorLeagueStrength,
                simulationResult.SimulationParams,
                simulationResult.Raport
            },
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(command);

        return;
    }
}
