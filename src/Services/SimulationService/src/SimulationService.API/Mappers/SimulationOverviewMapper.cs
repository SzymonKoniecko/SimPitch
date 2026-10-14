using System;
using System.Globalization;
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
        grpc.CreatedDate = dto.CreatedDate.ToString(GrpcDateFormat.Pattern, CultureInfo.InvariantCulture);
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
            NoiseFactor = dto.NoiseFactor,
            Model = dto.ModelType.ToString(),
        };

        if (dto.LeagueRoundId is Guid leagueRoundId && leagueRoundId != Guid.Empty) // null must not become ""
        {
            proto.LeagueRoundId = leagueRoundId.ToString();
        }

        if (dto.TargetLeagueRoundId is Guid targetLeagueRoundId && targetLeagueRoundId != Guid.Empty) // null must not become ""
        {
            proto.TargetLeagueRoundId = targetLeagueRoundId.ToString();
        }

        return proto;
    }
}
