using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.API.Mappers;

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
}
