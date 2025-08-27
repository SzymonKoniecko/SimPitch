using System;
using Dapper;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Infrastructure.Persistence.Write;

public class ScoreboardWriteRepository : IScoreboardWriteRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ScoreboardWriteRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }

    public async Task CreateScoreboardAsync(Scoreboard scoreboard, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        const string sql = @"
            INSERT INTO Scoreboards (Id, SimulationId, LeagueStrength, PriorLeagueStrength)
            VALUES (@Id, @SimulationId, @LeagueStrength, @PriorLeagueStrength)";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                scoreboard.Id,
                scoreboard.SimulationId,
                scoreboard.LeagueStrength,
                scoreboard.PriorLeagueStrength
            },
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(command);

        return;
    }
}
