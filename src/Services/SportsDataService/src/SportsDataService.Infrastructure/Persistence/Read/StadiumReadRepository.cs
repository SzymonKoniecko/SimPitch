using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class StadiumReadRepository : IStadiumReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;

    public StadiumReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Stadium> GetStadiumByIdAsync(Guid stadiumId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Stadium WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = stadiumId }
        );

        var stadium = await connection.QueryFirstOrDefaultAsync<Stadium>(command);

        if (stadium == null)
        {
            throw new KeyNotFoundException($"Stadium with Id '{stadiumId}' was not found.");
        }

        return stadium;
    }
    public async Task<IEnumerable<Stadium>> GetAllStadiumsAsync(CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Stadium ORDER BY Name";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return await connection.QueryAsync<Stadium>(command);
    }
    public async Task<bool> StadiumExistsAsync(Guid stadiumId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT COUNT(1) FROM Stadium WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = stadiumId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }
}
