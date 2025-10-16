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
        grpc.Title = dto.Title;
        grpc.CreatedDate = dto.CreatedDate.ToString();
        grpc.SimulationParams = SimulationParamsToProto(dto.SimulationParams);

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
            SeasonYears = { dto.SeasonYears },
            LeagueId = dto.LeagueId.ToString(),
            Iterations = dto.Iterations
        };

        if (dto.LeagueRoundId != Guid.Empty)
        {
            proto.LeagueRoundId = dto.LeagueRoundId.ToString();
        }

        return proto;
    }
}
