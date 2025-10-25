using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Infrastructure.Persistence.Read;

public class IterationResultReadRepository : IIterationResultReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public IterationResultReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IterationResult> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, SimulationId, IterationIndex, StartDate, ExecutionTime, TeamStrengths, 
            SimulatedMatchRounds, LeagueStrength, PriorLeagueStrength
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

    public async Task<IEnumerable<IterationResult>> GetIterationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, SimulationId, IterationIndex, StartDate, ExecutionTime, TeamStrengths
                SimulatedMatchRounds, LeagueStrength, PriorLeagueStrength
            FROM IterationResult
            WHERE SimulationId = @SimulationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var results = await connection.QueryAsync<IterationResult>(command);
        return results;
    }
}
