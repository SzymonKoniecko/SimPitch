using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;
namespace SportsDataService.Infrastructure.Persistence.Teams;
public class TeamReadRepository : ITeamReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TeamReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Team?> GetTeamByIdAsync(Guid teamId)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Team WHERE Id = @Id";
        var team = await connection.QueryFirstOrDefaultAsync<Team>(sql, new { Id = teamId });
        if (team == null)
        {
            throw new KeyNotFoundException($"Team with Id '{teamId}' was not found.");
        }
        return team;
    }

    public async Task<IEnumerable<Team>> GetAllTeamsAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Team order by Name";
        return await connection.QueryAsync<Team>(sql);
    }
}