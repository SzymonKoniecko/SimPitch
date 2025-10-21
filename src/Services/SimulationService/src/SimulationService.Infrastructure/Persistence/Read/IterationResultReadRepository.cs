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

    public async Task<IEnumerable<IterationResult>> GetIterationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        const string sql = @"
            SELECT Id, SimulationId, IterationIndex, StartDate, ExecutionTime, 
                SimulatedMatchRounds, LeagueStrength, PriorLeagueStrength, Raport
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
