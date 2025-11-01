using System;
using EngineService.Application.Common.Sorting;

namespace EngineService.Application.Mappers;

public static class EnumMapper
{
    public static SortingOptionEnum SortingOptionToEnum(string enumValue)
    {
        switch (enumValue)
        {
            case "CreatedDate":
                return SortingOptionEnum.CreatedDate;
            case "LowestExecutionTime":
                return SortingOptionEnum.LowestExecutionTime;
            case "Name":
                return SortingOptionEnum.Name;
            case "IterationResultNumber":
                return SortingOptionEnum.IterationResultNumber;
            case "Team":
                return SortingOptionEnum.Team;
            case "LeaderPoints":
                return SortingOptionEnum.LeaderPoints;
            default:
                throw new KeyNotFoundException($"Cannot map SortingOption to enum value: {enumValue}");
        }
    }
}
