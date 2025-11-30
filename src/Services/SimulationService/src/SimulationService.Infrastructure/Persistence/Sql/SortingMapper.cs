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
            SortingOptionEnum.Title => $"ORDER BY CAST(JSON_VALUE(SimulationParams, '$.Title') AS NVARCHAR) {order}",
            SortingOptionEnum.IterationResultNumber => $"ORDER BY CAST(JSON_VALUE(SimulationParams, '$.Iterations') AS INT) {order}",
            _ => $"ORDER BY CreatedDate {order}",
        };
    }

    public static string WhereClausule(SortingOptionEnum sortingOption, string condition = "")
    {
        return sortingOption switch
        {
            SortingOptionEnum.State => $" inner join SimulationDb.dbo.SimulationState ss On so.Id = ss.SimulationId where ss.State = '{condition}' ",
            SortingOptionEnum.Title => $" WHERE CAST(JSON_VALUE(SimulationParams, '$.Title') AS NVARCHAR) LIKE '%{condition}%' COLLATE SQL_Latin1_General_CP1_CI_AS ",
            SortingOptionEnum.League => $" WHERE JSON_VALUE(so.SimulationParams, '$.LeagueId') = '{condition}' COLLATE SQL_Latin1_General_CP1_CI_AS ",
            _ => $"WHERE 1=1",
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