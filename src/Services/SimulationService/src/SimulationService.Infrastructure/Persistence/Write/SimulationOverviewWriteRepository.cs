using System;
using Dapper;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Infrastructure.Persistence.Write;

public class SimulationOverviewWriteRepository : ISimulationOverviewWriteRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public SimulationOverviewWriteRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task CreateSimulationOverviewAsync(SimulationOverview simulationOverview, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO SimulationOverview 
            (Id, CreatedDate, SimulationParams)
            VALUES 
            (@Id, @CreatedDate, @SimulationParams);
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                simulationOverview.Id,
                simulationOverview.CreatedDate,
                simulationOverview.SimulationParams
            },
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(command);

        return;
    }
}
