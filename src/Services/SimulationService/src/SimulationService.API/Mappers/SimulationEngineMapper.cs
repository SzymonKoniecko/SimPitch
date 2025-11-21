using System;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Consts;

namespace SimulationService.API.Mappers;

public static class SimulationEngineMapper
{
    public static SimulationParamsDto SimulationEngineReqestToDto(RunSimulationEngineRequest request)
    {
        var dto = new SimulationParamsDto();

        dto.Title = request.SimulationParams.Title;
        dto.SeasonYears = request.SimulationParams.SeasonYears.ToList();
        dto.LeagueId = Guid.Parse(request.SimulationParams.LeagueId);
        dto.Iterations = request.SimulationParams.Iterations;
        dto.Seed = request.SimulationParams.Seed;
        dto.LeagueRoundId = request.SimulationParams.HasLeagueRoundId ? Guid.Parse(request.SimulationParams.LeagueRoundId) : Guid.Empty;
        dto.CreateScoreboardOnCompleteIteration = request.SimulationParams.HasCreateScoreboardOnCompleteIteration;
        dto.GamesToReachTrust = request.SimulationParams.HasGamesToReachTrust ? request.SimulationParams.GamesToReachTrust : SimulationConsts.GAMES_TO_REACH_TRUST;
        dto.ConfidenceLevel = request.SimulationParams.HasConfidenceLevel ? request.SimulationParams.ConfidenceLevel : SimulationConsts.SIMULATION_CONFIDENCE_LEVEL;
        dto.HomeAdvantage = request.SimulationParams.HasHomeAdvantage ? request.SimulationParams.HomeAdvantage : SimulationConsts.HOME_ADVANTAGE;
        dto.NoiseFactor = request.SimulationParams.HasNoiseFactor ? request.SimulationParams.NoiseFactor : SimulationConsts.NOISE_FACTOR;
        dto.ModelType = EnumMapper.StringtoModelTypeEnum(request.SimulationParams.Model);
        
        return dto;
    }

    public static SimulationStateGrpc StateToGrpc(SimulationStateDto dto)
    {
        var grpc = new SimulationStateGrpc();

        grpc.Id = dto.Id.ToString();
        grpc.SimulationId = dto.SimulationId.ToString();
        grpc.ProgressPercent = dto.ProgressPercent;
        grpc.LastCompletedIteration = dto.LastCompletedIteration;
        grpc.State = dto.State;
        grpc.UpdatedAt = dto.UpdatedAt.ToString();

        return grpc;
    }
}
