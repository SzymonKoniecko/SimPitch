using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Extensions;

public static class DeepCloneExtensions
{
    public static (List<MatchRound> matchRounds, Dictionary<Guid, List<TeamStrength>> teamStrengths) 
        CloneSimulationDataManual(
            List<MatchRound> matchRoundsToClone,
            Dictionary<Guid, List<TeamStrength>> teamStrengthDictToClone)
    {
        var clonedMatchRounds = new List<MatchRound>(matchRoundsToClone.Capacity);
        foreach (var match in matchRoundsToClone)
        {
            clonedMatchRounds.Add(match.Clone()); // Używa MatchRound.Clone()
        }

        var clonedTeamStrengths = new Dictionary<Guid, List<TeamStrength>>(teamStrengthDictToClone.Count);
        foreach (var kvp in teamStrengthDictToClone)
        {
            var clonedTeamList = new List<TeamStrength>(kvp.Value.Capacity);
            foreach (var teamStrength in kvp.Value)
            {
                clonedTeamList.Add(teamStrength.DeepClone()); // Używa TeamStrength.DeepClone()
            }
            clonedTeamStrengths[kvp.Key] = clonedTeamList;
        }

        return (clonedMatchRounds, clonedTeamStrengths);
    }
}
