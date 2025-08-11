using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;
namespace SportsDataService.Infrastructure.Persistence.Read;

public class TeamReadRepository : ITeamReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TeamReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Team?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Team WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = teamId },
            cancellationToken: cancellationToken
        );

        var team = await connection.QueryFirstOrDefaultAsync<Team>(command);

        if (team == null)
        {
            throw new KeyNotFoundException($"Team with Id '{teamId}' was not found.");
        }

        return team;
    }

    public async Task<IEnumerable<Team>> GetAllTeamsAsync(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Team order by Name";

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return await connection.QueryAsync<Team>(command);
    }
    public async Task<bool> TeamExistsAsync(Guid teamId, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT COUNT(1) FROM Team WHERE Id = @Id";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = teamId },
            cancellationToken: cancellationToken
        );

        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }
}