using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Infrastructure.Persistence.Sql;

public static class SortingMapper
{
    public static string OrderAndFilterToSqlColumnIterationResults(string sortingOption, string direction)
    {
        return sortingOption switch
        {
            "CreatedDate" => $"ORDER BY StartDate {direction}",
            "ExecutionTime" => $"ORDER BY ExecutionTime {direction}",
            "IterationResultNumber" => $"ORDER BY IterationIndex {direction}",
            _ => $"ORDER BY StartDate {direction}",
        };
    }

    public static string ToSqlColumnSimulationOverviews(string sortingOption, string direction)
    {
        return sortingOption switch
        {
            "CreatedDate" => $"ORDER BY CreatedDate {direction}",
            "ExecutionTime" => $"ORDER BY ExecutionTime {direction}",
            "IterationResultNumber" => $"ORDER BY CAST(JSON_VALUE(SimulationParams, '$.Iterations') AS INT) {direction}",
            _ => $"ORDER BY CreatedDate {direction}",
        };
    }

    public static string GetSortDirection(string condition)
    {
        return condition switch
        {
            "ASC" => "ASC",
            "DESC" => "DESC",
            _ => "DESC"
        };
    }
}