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
        if (result == null) throw new KeyNotFoundException("No simulation overviews for given ID");
        return result;
    }

    public async Task<IEnumerable<SimulationOverview>> GetSimulationOverviewsAsync(
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT *
            FROM SimulationOverview
            ORDER BY CreatedDate
            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Offset = offset, Limit = limit },
            cancellationToken: cancellationToken
        );

        var results = await connection.QueryAsync<SimulationOverview>(command);
        return results;
    }

    public async Task<int> GetSimulationOverviewCountAsync(CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = "SELECT COUNT(*) FROM SimulationOverview;";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return await connection.ExecuteScalarAsync<int>(command);
    }
}
