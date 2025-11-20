using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Infrastructure.Persistence.Read;

public class LeagueReadRepository : ILeagueReadRepository
{
    private readonly IDbConnectionFactory _DbConnectionFactory;
    private readonly ILeagueStrengthReadRepository _leagueStrengthReadRepository;

    public LeagueReadRepository(IDbConnectionFactory dbConnectionFactory, ILeagueStrengthReadRepository leagueStrengthReadRepository)
    {
        if (dbConnectionFactory == null)
        {
            throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        _DbConnectionFactory = dbConnectionFactory;
        _leagueStrengthReadRepository = leagueStrengthReadRepository;
    }
    public async Task<IEnumerable<League>> GetLeaguesByCountryIdAsync(Guid countryId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM League WHERE CountryId = @CountryId";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { CountryId = countryId },
            cancellationToken: cancellationToken
        );

        var leagues = await connection.QueryAsync<League>(command);

        if (leagues == null)
        {
            throw new KeyNotFoundException($"Leagues with CountryId '{countryId}' was not found.");
        }
        
        foreach (var league in leagues)
        {
            league.Strengths = await _leagueStrengthReadRepository.GetLeagueStrengthsByLeagueIdAsync(league.Id, cancellationToken);
        }

        return leagues;
    }
    public async Task<IEnumerable<League>> GetAllLeaguesAsync(CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM League ORDER BY Name";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );
        var leagues = await connection.QueryAsync<League>(command);
        
        foreach (var league in leagues)
        {
            league.Strengths = await _leagueStrengthReadRepository.GetLeagueStrengthsByLeagueIdAsync(league.Id, cancellationToken);
        }
        return leagues;
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

    public async Task<League> GetByIdAsync(Guid leagueId, CancellationToken cancellationToken)
    {
        using var connection = _DbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM League WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = leagueId },
            cancellationToken: cancellationToken
        );

        var league = await connection.QuerySingleOrDefaultAsync<League>(command);

        if (league == null)
        {
            throw new KeyNotFoundException($"League with Id '{leagueId}' was not found.");
        }
        league.Strengths = await _leagueStrengthReadRepository.GetLeagueStrengthsByLeagueIdAsync(leagueId, cancellationToken);
        return league;
    }
}
