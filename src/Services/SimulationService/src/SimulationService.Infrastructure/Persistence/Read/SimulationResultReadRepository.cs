using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Infrastructure.Persistence.Read;

public class SimulationResultReadRepository : ISimulationResultReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public SimulationResultReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<SimulationResult>> GetSimulationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        const string sql = @"
            SELECT Id, SimulationId, SimulationIndex, StartDate, ExecutionTime, 
                SimulatedMatchRounds, LeagueStrength, PriorLeagueStrength, SimulationParams, Raport
            FROM SimulationResult
            WHERE SimulationId = @SimulationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var results = await connection.QueryAsync<SimulationResult>(command);
        return results;
    }
}
