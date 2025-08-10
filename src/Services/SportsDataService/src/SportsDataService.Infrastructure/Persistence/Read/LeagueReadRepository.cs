using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class LeagueReadRepository : ILeagueReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;
    public LeagueReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }
    public async Task<League> GetLeagueByIdAsync(Guid leagueId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM League WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = leagueId },
            cancellationToken: cancellationToken
        );

        var league = await connection.QueryFirstOrDefaultAsync<League>(command);

        if (league == null)
        {
            throw new KeyNotFoundException($"League with Id '{leagueId}' was not found.");
        }

        return league;
    }
    public async Task<IEnumerable<League>> GetAllLeaguesAsync(CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM League ORDER BY Name";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return await connection.QueryAsync<League>(command);
    }
    public async Task<bool> LeagueExistsAsync(Guid leagueId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT COUNT(1) FROM League WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = leagueId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }
}
