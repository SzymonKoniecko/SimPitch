using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Infrastructure.Persistence.Read;

public class SimulationOverviewReadRepository : ISimulationOverviewReadRepository
{
private readonly IDbConnectionFactory _dbConnectionFactory;
    public SimulationOverviewReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }


    public async Task<SimulationOverview> GetSimulationOverviewByIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT *
            FROM SimulationOverview
            WHERE Id = @SimulationId;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { SimulationId = simulationId },
            cancellationToken: cancellationToken
        );

        var result = await connection.QuerySingleOrDefaultAsync<SimulationOverview>(command);
        if (result == null) throw new Exception("No simulation overviews for given ID");
        return result;
    }
    
    public async Task<IEnumerable<SimulationOverview>> GetSimulationOverviewsAsync(CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT *
            FROM SimulationOverview;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        var results = await connection.QueryAsync<SimulationOverview>(command);
        return results;
    }
}
