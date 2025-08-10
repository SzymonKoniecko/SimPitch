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
    public async Task<Guid> CreateStadiumAsync(Stadium stadium, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Stadium (Id, Name, Capacity, Location)
            VALUES (@Id, @Name, @Capacity, @CreatedAt, @UpdatedAt)
            OUTPUT INSERTED.*;
        ";

        // Upewniamy się, że Id jest ustawione
        if (stadium.Id == Guid.Empty)
            stadium.Id = Guid.NewGuid();

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                stadium.Id,
                stadium.Name,
                stadium.Capacity,
                stadium.CreatedAt,
                stadium.UpdatedAt
            },
            cancellationToken: cancellationToken
        );

        var createdStadium = await connection.QueryFirstOrDefaultAsync<Stadium>(command);
        if (createdStadium == null)
            throw new Exception("Failed to create stadium.");

        return createdStadium.Id;
    }

}
