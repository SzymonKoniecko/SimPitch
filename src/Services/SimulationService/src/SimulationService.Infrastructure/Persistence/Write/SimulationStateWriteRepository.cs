using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Infrastructure.Persistence.Write;

public class SimulationStateWriteRepository : ISimulationStateWriteRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SimulationStateWriteRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Create or update SimulationState record (Upsert).
    /// </summary>
    public async Task UpdateOrCreateAsync(SimulationState state, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
            MERGE INTO dbo.SimulationState AS target
            USING (SELECT @SimulationId AS SimulationId) AS source
            ON target.SimulationId = source.SimulationId
            WHEN MATCHED THEN 
                UPDATE SET 
                    LastCompletedIteration = @LastCompletedIteration,
                    ProgressPercent = @ProgressPercent,
                    [State] = @State,
                    UpdatedAt = @UpdatedAt
            WHEN NOT MATCHED THEN 
                INSERT (Id, SimulationId, LastCompletedIteration, ProgressPercent, [State], UpdatedAt)
                VALUES (@Id, @SimulationId, @LastCompletedIteration, @ProgressPercent, @State, @UpdatedAt);";

        await connection.ExecuteAsync(sql, new
        {
            state.Id,
            state.SimulationId,
            state.LastCompletedIteration,
            state.ProgressPercent,
            State = state.State.ToString(),
            state.UpdatedAt
        });
    }

    /// <summary>
    /// Change only the SimulationStatus (and UpdatedAt).
    /// </summary>
    public async Task ChangeStatusAsync(Guid simulationId, SimulationStatus newStatus, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"
            UPDATE dbo.SimulationState
            SET [State] = @State,
                UpdatedAt = @UpdatedAt
            WHERE SimulationId = @SimulationId;";

        await connection.ExecuteAsync(sql, new
        {
            SimulationId = simulationId,
            State = newStatus.ToString(),
            UpdatedAt = DateTime.UtcNow
        });
    }
}
