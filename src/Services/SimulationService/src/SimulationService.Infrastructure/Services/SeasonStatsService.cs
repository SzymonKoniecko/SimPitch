using System;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Infrastructure.Services;

public class SeasonStatsService : ISeasonStatsService
{
    public async Task<SeasonStats> CalculateSeasonStatsForCurrentSeasonAsync(SeasonEnum seasonEnum)
    {
        return null;
    }
}
