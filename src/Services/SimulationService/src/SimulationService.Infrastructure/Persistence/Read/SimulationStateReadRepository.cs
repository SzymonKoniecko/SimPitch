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

    public async Task<bool> IsSimulationStateCancelled(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Count(*)
            FROM dbo.SimulationState
            WHERE SimulationId = @SimulationId AND [State] = 'Cancelled';
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<SimulationState> GetSimulationStateBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
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

    public async Task<bool> SimulationStateBySimulationIdExistsAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        string sql = @"SELECT COUNT(1)
            FROM dbo.SimulationState
            WHERE SimulationId = @SimulationId;";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }
}
