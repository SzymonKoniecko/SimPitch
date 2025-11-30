using System;
using EngineService.Application.Common.Sorting;
using EngineService.Domain.Enums;

namespace EngineService.Application.Mappers;

public static class EnumMapper
{
    public static SortingOptionEnum SortingOptionToEnum(string enumValue)
    {
        switch (enumValue)
        {
            case "CreatedDate":
                return SortingOptionEnum.CreatedDate;
            case "ExecutionTime":
                return SortingOptionEnum.ExecutionTime;
            case "Title":
                return SortingOptionEnum.Title;
            case "IterationResultNumber":
                return SortingOptionEnum.IterationResultNumber;
            case "LeaderPoints": // will be sorted in the closest handler
                return SortingOptionEnum.LeaderPoints;
            case "State":
                return SortingOptionEnum.State;
            default:
                throw new KeyNotFoundException($"Cannot map SortingOption to enum value: {enumValue}");
        }
    }
}
