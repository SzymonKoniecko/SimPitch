using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Infrastructure.Persistence.Read;

public class SimulationStateReadRepository : ISimulationStateReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public SimulationStateReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }


    public async Task<SimulationState> GetSimulationStateByIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        const string sql = @"
            SELECT 
                Id,
                SimulationId,
                LastCompletedIteration,
                ProgressPercent,
                [State],
                UpdatedAt
            FROM dbo.SimulationState
            WHERE SimulationId = @SimulationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var result = await connection.QuerySingleOrDefaultAsync<SimulationState>(command);

        if (result is null)
            throw new KeyNotFoundException($"No simulation state found for SimulationId = {simulationId}");

        return result;
    }
}
