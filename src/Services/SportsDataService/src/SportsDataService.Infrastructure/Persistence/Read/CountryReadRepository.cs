using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;


namespace SportsDataService.Infrastructure.Persistence.Read;

public class CountryReadRepository : ICountryReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;
    public CountryReadRepository(IDbConnectionFactory dbConnectionFactory)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
    }
    public async Task<Country> GetCountryByIdAsync(Guid countryId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Country WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = countryId },
            cancellationToken: cancellationToken
        );

        var country = await connection.QueryFirstOrDefaultAsync<Country>(command);

        if (country == null)
        {
            throw new KeyNotFoundException($"Country with Id '{countryId}' was not found.");
        }

        return country;
    }
    public async Task<IEnumerable<Country>> GetAllCountriesAsync(CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Country ORDER BY Code";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return await connection.QueryAsync<Country>(command);
    }
    public async Task<bool> CountryExistsAsync(Guid countryId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT COUNT(1) FROM Country WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = countryId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }
}
