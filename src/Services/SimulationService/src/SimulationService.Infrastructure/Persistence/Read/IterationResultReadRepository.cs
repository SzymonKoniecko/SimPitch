using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.ValueObjects;
using SimulationService.Infrastructure.Builders;

namespace SimulationService.Infrastructure.Persistence.Read;

public class IterationResultReadRepository : IIterationResultReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ISimulationStateReadRepository _simulationStateReadRepository;

    public IterationResultReadRepository(
        IDbConnectionFactory dbConnectionFactory,
        ISimulationStateReadRepository simulationStateReadRepository)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _simulationStateReadRepository = simulationStateReadRepository;
    }

    public async Task<IterationResult> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, SimulationId, IterationIndex, StartDate, ExecutionTime, TeamStrengths, 
            SimulatedMatchRounds
            FROM IterationResult
            WHERE Id = @IterationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { IterationId = iterationId },
            cancellationToken: cancellationToken
        );


        var result = await connection.QuerySingleOrDefaultAsync<IterationResult>(command);
        if (result == null) throw new KeyNotFoundException("No iteration result for given ID");

        return result;
    }

    public async Task<IEnumerable<IterationResult>> GetIterationResultsBySimulationIdAsync(
        Guid simulationId,
        PagedRequest pagedRequest,
        CancellationToken cancellationToken)
    {
        var simulationState = await _simulationStateReadRepository
            .GetSimulationStateBySimulationIdAsync(simulationId, cancellationToken);

        using var connection = _dbConnectionFactory.CreateConnection();

        var command = CustomSqlCommandBuilder.BuildPagedIterationResultsQuery(
            simulationId,
            simulationState.LastCompletedIteration,
            pagedRequest,
            cancellationToken);

        var results = await connection.QueryAsync<IterationResult>(command);

        return results;
    }

    public async Task<int> GetIterationResultsCountBySimulationId_AndStateAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var simulationState = await _simulationStateReadRepository // idk why are using it (maybe to prevent the issue with checking the simulation results WHEN SIMULATION IS IN PROGRESS)
            .GetSimulationStateBySimulationIdAsync(simulationId, cancellationToken);

        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT COUNT(*)
            FROM IterationResult
            WHERE SimulationId = @SimulationId
            AND IterationIndex <= @MaxIndex;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                SimulationId = simulationId,
                MaxIndex = simulationState.LastCompletedIteration
            },
            cancellationToken: cancellationToken
        );

        return await connection.ExecuteScalarAsync<int>(command);
    }

    public async Task<int> GetIterationResultsCountBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {

        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT COUNT(*)
            FROM IterationResult
            WHERE SimulationId = @SimulationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                SimulationId = simulationId,
            },
            cancellationToken: cancellationToken
        );

        return await connection.ExecuteScalarAsync<int>(command);
    }

    public async Task<IterationResult> GetLatestIterationResultBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {

        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, SimulationId, IterationIndex, StartDate, ExecutionTime, TeamStrengths, 
            SimulatedMatchRounds
            FROM IterationResult
            WHERE Id = @SimulationId
            ORDER BY IterationIndex DESC;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );


        var result = await connection.QuerySingleOrDefaultAsync<IterationResult>(command);
        if (result == null) throw new KeyNotFoundException("No latest iteration result for given ID");

        return result;
    }
}
