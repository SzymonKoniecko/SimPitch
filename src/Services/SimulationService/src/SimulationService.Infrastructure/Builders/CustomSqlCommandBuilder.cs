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
        var order = SortingMapper.GetSortDirection(request.SortingMethod.Order);
        var sqlOrderFilter = SortingMapper.OrderAndFilterToSqlColumnIterationResults(request.SortingMethod.SortingOption, order);

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
                Offset = request.Offset,
                Limit = request.PageSize
            },
            cancellationToken: cancellationToken
        );
    }

    public static CommandDefinition BuildPagedSimulationOverviewsQuery(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        var order = SortingMapper.GetSortDirection(request.SortingMethod.Order);

        var whereClausule = SortingMapper.WhereClausule(request.SortingMethod.SortingOption, request.SortingMethod.Condition);
        var sqlOrderFilter = SortingMapper.ToSqlColumnSimulationOverviews(request.SortingMethod.SortingOption, order);

        string sql = $@"
        SET @Offset = ISNULL(@Offset, 0);
        IF @Offset < 0 SET @Offset = 0;
            SELECT so.Id, so.CreatedDate, so.SimulationParams, so.LeagueStrengthsJSON, so.PriorLeagueStrength 
            FROM SimulationOverview so
            {whereClausule}
            {sqlOrderFilter} 
            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
        ";
        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { Offset = request.Offset, Limit = request.PageSize },
            cancellationToken: cancellationToken
        );

        return command;
    }

    public static CommandDefinition BuildPagedSimulationOverviewsQueryCount(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        var order = SortingMapper.GetSortDirection(request.SortingMethod.Order);

        var whereClausule = SortingMapper.WhereClausule(request.SortingMethod.SortingOption, request.SortingMethod.Condition);

        string sql = $@"
            SELECT Count(*) 
            FROM SimulationOverview so
            {whereClausule} ;
        ";
        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken
        );

        return command;
    }
}
