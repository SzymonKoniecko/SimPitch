using System;
using Dapper;
using SimulationService.Domain.ValueObjects;
using SimulationService.Infrastructure.Persistence.Sql;

namespace SimulationService.Infrastructure.Builders;

public class CustomSqlCommandBuilder
{
    public static CommandDefinition BuildPagedIterationResultsQuery(
        Guid simulationId,
        int maxIndex,
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        var offset = (request.PageNumber - 1) * request.PageSize;
        var limit = request.PageSize;

        var direction = SortingMapper.GetSortDirection(request.SortingMethod.Direction);
        var sqlOrderFilter = SortingMapper.OrderAndFilterToSqlColumnIterationResults(request.SortingMethod.SortingOption, direction);

        var sql = $@"
        SET @Offset = ISNULL(@Offset, 0);
IF @Offset < 0 SET @Offset = 0;
            SELECT *
            FROM IterationResult
            WHERE SimulationId = @SimulationId
              AND IterationIndex <= @MaxIndex
            {sqlOrderFilter} 
            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
        ";

        return new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                SimulationId = simulationId,
                MaxIndex = maxIndex,
                Offset = offset,
                Limit = limit
            },
            cancellationToken: cancellationToken
        );
    }

    public static CommandDefinition BuildPagedSimulationOverviewsQuery(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        var offset = (request.PageNumber - 1) * request.PageSize;
        var limit = request.PageSize;
        
        var direction = SortingMapper.GetSortDirection(request.SortingMethod.Direction);

        var sqlOrderFilter = SortingMapper.ToSqlColumnSimulationOverviews(request.SortingMethod.SortingOption, direction);

        string sql = $@"
        SET @Offset = ISNULL(@Offset, 0);
IF @Offset < 0 SET @Offset = 0;
            SELECT *
            FROM SimulationOverview
            {sqlOrderFilter}
            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
        ";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Offset = offset, Limit = limit },
            cancellationToken: cancellationToken
        );

        return command;
    }
}
