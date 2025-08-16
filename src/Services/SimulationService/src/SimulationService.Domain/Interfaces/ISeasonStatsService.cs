using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.Interfaces;

public interface ISeasonStatsService
{
    Task<SeasonStats> CalculateSeasonStatsForCurrentSeasonAsync(SeasonEnum seasonEnum);
}
