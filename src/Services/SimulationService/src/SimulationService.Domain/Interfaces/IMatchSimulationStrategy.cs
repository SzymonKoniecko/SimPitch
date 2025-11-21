using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Interfaces;

public interface IMatchSimulationStrategy
{
        (int HomeGoals, int AwayGoals) SimulateMatch(
            TeamStrength homeTeam, 
            TeamStrength awayTeam, 
            float leagueAverageGoals,
            SimulationParams simParams,
            Random rng);
}
