using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.ValueObjects;
using SimulationService.Infrastructure.Builders;

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
        if (result == null)
        {
            var retryResult = await connection.QuerySingleOrDefaultAsync<SimulationOverview>(command);
            if (retryResult == null)
            {
                throw new KeyNotFoundException("No simulation overviews for given ID");
            }
            return retryResult;
        }
        return result;
    }

    public async Task<IEnumerable<SimulationOverview>> GetSimulationOverviewsAsync(
        PagedRequest pagedRequest,
        CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = CustomSqlCommandBuilder.BuildPagedSimulationOverviewsQuery(pagedRequest, cancellationToken);

        var results = await connection.QueryAsync<SimulationOverview>(command);
        return results;
    }

    public async Task<int> GetSimulationOverviewCountAsync(PagedRequest pagedRequest, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = CustomSqlCommandBuilder.BuildPagedSimulationOverviewsQueryCount(pagedRequest, cancellationToken);

        return await connection.ExecuteScalarAsync<int>(command);
    }
}
