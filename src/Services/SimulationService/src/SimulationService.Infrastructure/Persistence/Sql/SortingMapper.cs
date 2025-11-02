using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Infrastructure.Persistence.Sql;

public static class SortingMapper
{
    public static string OrderAndFilterToSqlColumnIterationResults(SortingOptionEnum sortingOption, string order)
    {
        return sortingOption switch
        {
            SortingOptionEnum.CreatedDate => $"ORDER BY StartDate {order}",
            SortingOptionEnum.ExecutionTime => $"ORDER BY ExecutionTime {order}",
            SortingOptionEnum.IterationResultNumber => $"ORDER BY IterationIndex {order}",
            SortingOptionEnum.LeaderPoints => $"ORDER BY StartDate {order}",
            _ => $"ORDER BY StartDate {order}",
        };
    }

    public static string ToSqlColumnSimulationOverviews(SortingOptionEnum sortingOption, string order)
    {
        return sortingOption switch
        {
            SortingOptionEnum.CreatedDate => $"ORDER BY CreatedDate {order}",
            SortingOptionEnum.Name => $"ORDER BY Title {order}",
            SortingOptionEnum.IterationResultNumber => $"ORDER BY CAST(JSON_VALUE(SimulationParams, '$.Iterations') AS INT) {order}",
            _ => $"ORDER BY CreatedDate {order}",
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