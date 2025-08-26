using System;
using Dapper;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Write;

namespace SportsDataService.Infrastructure.Persistence.Write;

public class StadiumWriteRepository : IStadiumWriteRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public StadiumWriteRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task CreateStadiumAsync(Stadium stadium, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Stadium (Id, Name, Capacity)
            VALUES (@Id, @Name, @Capacity);
        ";

        if (stadium.Id == Guid.Empty)
            stadium.Id = Guid.NewGuid();

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                stadium.Id,
                stadium.Name,
                stadium.Capacity
            },
            cancellationToken: cancellationToken
        );

        await connection.QueryFirstOrDefaultAsync<string>(command);

        return ;
    }

}
