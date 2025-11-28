using System;
using SimulationService.Application.Common.Sorting;
using SimulationService.Domain.Enums;

namespace SimulationService.Application.Mappers;

public static class EnumMapper
{
    
    public static string SeasonEnumToString(this SeasonEnum seasonEnum)
    {
        if (SeasonEnum.Season2022_2023 == seasonEnum)
            return "2022/2023";
        if (SeasonEnum.Season2023_2024 == seasonEnum)
            return "2023/2024";
        if (SeasonEnum.Season2024_2025 == seasonEnum)
            return "2024/2025";
        if (SeasonEnum.Season2025_2026 == seasonEnum)
            return "2025/2026";
        throw new ArgumentException($"Invalid season enum type. Provided {seasonEnum}");
    }

    public static SeasonEnum StringtoSeasonEnum(this string seasonEnum)
    {
        if ("2022/2023" == seasonEnum)
            return SeasonEnum.Season2022_2023;
        if ("2023/2024" == seasonEnum)
            return SeasonEnum.Season2023_2024;
        if ("2024/2025" == seasonEnum)
            return SeasonEnum.Season2024_2025;
        if ("2025/2026" == seasonEnum)
            return SeasonEnum.Season2025_2026;
        throw new ArgumentException($"Invalid season string type. Provided {seasonEnum}");
    }
    /// <summary>
    /// IDK why but we needs it for calculation of future MatchRounds
    /// </summary>
    public static SeasonEnum GetPreviousSeason(SeasonEnum seasonEnum)
    {
        if (SeasonEnum.Season2022_2023 == seasonEnum)
            return SeasonEnum.Season2022_2023; // its the last
        if (SeasonEnum.Season2023_2024 == seasonEnum)
            return SeasonEnum.Season2022_2023;
        if (SeasonEnum.Season2024_2025 == seasonEnum)
            return SeasonEnum.Season2023_2024;
        if (SeasonEnum.Season2025_2026 == seasonEnum)
            return SeasonEnum.Season2024_2025;
        throw new ArgumentException($"Invalid season enum type. Provided {seasonEnum}");
    }

    public static Domain.Enums.SortingOptionEnum SortingOptionToEnum(string enumValue)
    {
        switch (enumValue)
        {
            case "CreatedDate":
                return Domain.Enums.SortingOptionEnum .CreatedDate;
            case "ExecutionTime":
                return Domain.Enums.SortingOptionEnum .ExecutionTime;
            case "Name":
                return Domain.Enums.SortingOptionEnum .Name;
            case "IterationResultNumber":
                return Domain.Enums.SortingOptionEnum .IterationResultNumber;
            case "LeaderPoints":
                return Domain.Enums.SortingOptionEnum .LeaderPoints;
            default:
                throw new KeyNotFoundException($"Cannot map SortingOption to enum value: {enumValue}");
        }
    }

    internal static string StringedIntToSeasonString(string seasonYear)
    {
        int value = int.Parse(seasonYear);
        SeasonEnum season = (SeasonEnum)value;

        if (SeasonEnum.Season2022_2023 == season)
            return "2022/2023";
        if (SeasonEnum.Season2023_2024 == season)
            return "2023/2024";
        if (SeasonEnum.Season2024_2025 == season)
            return "2024/2025";
        if (SeasonEnum.Season2025_2026 == season)
            return "2025/2026";
        
        throw new KeyNotFoundException($"Cannot map season enum from his INT value to enum value: int?->{seasonYear}//");
    }

    public static SimulationModelType StringtoModelTypeEnum(this string model)
    {
        if ("StandardPoisson" == model)
            return SimulationModelType.StandardPoisson;
        if ("DixonColes" == model)
            return SimulationModelType.DixonColes;
        if ("BivariatePoisson" == model)
            return SimulationModelType.BivariatePoisson;
        if ("Advanced" == model)
            return SimulationModelType.Advanced;
        throw new ArgumentException($"Invalid simulation model string type. Provided {model}");
    }
}
