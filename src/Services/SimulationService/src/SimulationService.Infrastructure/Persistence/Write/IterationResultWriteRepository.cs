using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Infrastructure.Persistence.Write;

public class IterationResultWriteRepository : IIterationResultWriteRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public IterationResultWriteRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task CreateIterationResultAsync(IterationResult IterationResult, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO IterationResult 
            (Id, SimulationId, SimulationIndex, StartDate, ExecutionTime, SimulatedMatchRounds, LeagueStrength, PriorLeagueStrength, SimulationParams, Raport)
            VALUES 
            (@Id, @SimulationId, @SimulationIndex, @StartDate, @ExecutionTime, @SimulatedMatchRounds, @LeagueStrength, @PriorLeagueStrength, @SimulationParams, @Raport);
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                IterationResult.Id,
                IterationResult.SimulationId,
                IterationResult.IterationIndex,
                IterationResult.StartDate,
                IterationResult.ExecutionTime,
                IterationResult.SimulatedMatchRounds,
                IterationResult.LeagueStrength,
                IterationResult.PriorLeagueStrength,
                IterationResult.SimulationParams,
                IterationResult.Raport
            },
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(command);

        return;
    }
}
