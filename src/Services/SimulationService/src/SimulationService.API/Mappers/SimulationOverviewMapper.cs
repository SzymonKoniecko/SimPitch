using System;
using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.API.Mappers;

public static class SimulationOverviewMapper
{
    public static SimulationOverviewGrpc ToProto(SimulationOverviewDto dto)
    {
        var grpc = new SimulationOverviewGrpc();

        grpc.Id = dto.Id.ToString();
        grpc.CreatedDate = dto.CreatedDate.ToString();
        grpc.SimulationParams = SimulationParamsToProto(dto.SimulationParams);
        grpc.LeagueStrengths = JsonConvert.SerializeObject(dto.LeagueStrengths);
        grpc.PriorLeagueStrength = dto.PriorLeagueStrength;

        return grpc;
    } 
    
    public static SimulationParamsGrpc SimulationParamsToProto(SimulationParamsDto dto)
    {
        if (dto == null)
        {
            return null;
        }
        
        var proto = new SimulationParamsGrpc
        {
            Title = dto.Title,
            SeasonYears = { dto.SeasonYears },
            LeagueId = dto.LeagueId.ToString(),
            Iterations = dto.Iterations,
            CreateScoreboardOnCompleteIteration = dto.CreateScoreboardOnCompleteIteration,
            Seed = dto.Seed,
            GamesToReachTrust = dto.GamesToReachTrust,
            ConfidenceLevel = dto.ConfidenceLevel,
            HomeAdvantage = dto.HomeAdvantage,
            NoiseFactor = dto.NoiseFactor
        };

        if (dto.LeagueRoundId != Guid.Empty)
        {
            proto.LeagueRoundId = dto.LeagueRoundId.ToString();
        }


        return proto;
    }
}
