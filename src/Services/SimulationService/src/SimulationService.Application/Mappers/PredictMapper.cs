using System;
using SimulationService.Application.Features.Predict.DTOs;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Mappers;

public static class PredictMapper
{
    public static PredictRequestDto CreatePredictRequest(SimulationOverview overview, SimulationContent simulationContent)
    {
        var dto = new PredictRequestDto();
        dto.SimulationId = overview.Id;
        dto.LeagueId = simulationContent.SimulationParams.LeagueId;
        dto.IterationCount = simulationContent.SimulationParams.Iterations;
        dto.TeamStrengths = simulationContent.TeamsStrengthDictionary;
        dto.MatchesToSimulate = simulationContent.MatchRoundsToSimulate;
        dto.TrainUntilRoundNumber = 18;
        dto.LeagueAverangeStrength = overview.PriorLeagueStrength;
        dto.Seed = simulationContent.SimulationParams.Seed;
        dto.TrainRatio = null;
        dto.GamesToReachTrust = simulationContent.SimulationParams.GamesToReachTrust;
        return dto;
    }
}
