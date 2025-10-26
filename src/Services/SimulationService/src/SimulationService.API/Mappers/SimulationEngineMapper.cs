using System;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.API.Mappers;

public static class SimulationEngineMapper
{
    public static SimulationParamsDto SimulationEngineReqestToDto(RunSimulationEngineRequest request)
    {
        var dto = new SimulationParamsDto();
        dto.SeasonYears = request.SimulationParams.SeasonYears.ToList();
        dto.LeagueId = Guid.Parse(request.SimulationParams.LeagueId);
        dto.Iterations = request.SimulationParams.Iterations;
        dto.LeagueRoundId = request.SimulationParams.HasLeagueRoundId ? Guid.Parse(request.SimulationParams.LeagueRoundId) : Guid.Empty;

        return dto;
    }

    public static SimulationStateGrpc StateToGrpc(Domain.Entities.SimulationState state)
    {
        var grpc = new SimulationStateGrpc();

        switch (state.SimulationStatus)
        {
            case Domain.Enums.SimulationStatus.Pending:
                grpc.SimulationStatus = "Pending";
                break;
            case Domain.Enums.SimulationStatus.Running:
                grpc.SimulationStatus = "Running";
                break;
            case Domain.Enums.SimulationStatus.Completed:
                grpc.SimulationStatus = "Completed";
                break;
            case Domain.Enums.SimulationStatus.Cancelled:
                grpc.SimulationStatus = "Cancelled";
                break;
            case Domain.Enums.SimulationStatus.Failed:
                grpc.SimulationStatus = "Failed";
                break;
            default:
                throw new KeyNotFoundException("Missing simulation state value");
        }
        grpc.Progress = state.ProgressPercent;
        grpc.UpdatedAt = state.UpdatedAt.ToString();

        return grpc;
    }
}
