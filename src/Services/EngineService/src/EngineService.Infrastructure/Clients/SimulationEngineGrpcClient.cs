using System;
using System.Globalization;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using EngineService.Domain.ValueObjects;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;

namespace EngineService.Infrastructure.Clients;

public class SimulationEngineGrpcClient : ISimulationEngineGrpcClient
{
    private readonly SimulationEngineService.SimulationEngineServiceClient _client;

    public SimulationEngineGrpcClient(SimulationEngineService.SimulationEngineServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<string> CreateSimulationAsync(SimulationParamsDto simulationParamsDto, CancellationToken cancellationToken)
    {
        var request = new RunSimulationEngineRequest();
        request.SimulationParams = ToProto(simulationParamsDto);

        var response = await _client.RunSimulationAsync(request, cancellationToken: cancellationToken);

        return response.SimulationId;
    }

    public async Task<(List<SimulationOverviewDto>, PagedResponseDetails)> GetPagedSimulationOverviewsAsync(PagedRequest pagedRequest, CancellationToken cancellationToken)
    {
        var request = new PagedRequestGrpc
        {
            Offset = pagedRequest.Offset,
            Limit = pagedRequest.PageSize,
            SortingMethod = new SortingMethodGrpc
            {
                SortingOption = pagedRequest.SortingMethod.SortingOption.ToString(),
                Condition = pagedRequest.SortingMethod.Condition,
                Order = pagedRequest.SortingMethod.Order
            }
        };

        var response = await _client.GetAllSimulationOverviewsAsync(request, cancellationToken: cancellationToken);

        return (
            response.Items.Select(x => ToDto(x)).ToList(),
            MapToDto(response.Paged, pagedRequest));
    }

    private PagedResponseDetails MapToDto(PagedResponseGrpc grpc, PagedRequest pagedRequest)
    {
        var details = new PagedResponseDetails();

        details.TotalCount = grpc.TotalCount;
        details.PageNumber = (pagedRequest.Offset / pagedRequest.PageSize) + 1;
        details.PageSize = pagedRequest.PageSize;
        details.SortingOption = grpc.SortingOption;
        details.Order = grpc.SortingOrder;

        return details;
    }

    public async Task<SimulationOverviewDto> GetSimulationOverviewBySimulationId(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new GetSimulationOverviewByIdRequest();
        request.SimulationId = simulationId.ToString();

        var response = await _client.GetSimulationOverviewByIdAsync(request, cancellationToken: cancellationToken);

        return ToDto(response.SimulationOverview);
    }

    public async Task<SimulationStateDto> GetSimulationStateAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new GetSimulationStateByIdRequest();
        request.SimulationId = simulationId.ToString();

        var response = await _client.GetSimulationStateByIdAsync(request, cancellationToken: cancellationToken);

        return StateGrpcToDto(response.SimulationState);
    }

    public async Task<string> StopSimulationAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new StopSimulationRequest();
        request.SimulationId = simulationId.ToString();

        var response = await _client.StopSimulationByIdAsync(request, cancellationToken: cancellationToken);

        return response.Status;
    }

    /// Mappers

    private static SimulationStateDto StateGrpcToDto(SimulationStateGrpc stateGrpc)
    {
        var dto = new SimulationStateDto();

        dto.Id = Guid.Parse(stateGrpc.Id);
        dto.SimulationId = Guid.Parse(stateGrpc.SimulationId);
        dto.ProgressPercent = stateGrpc.ProgressPercent;
        dto.LastCompletedIteration = stateGrpc.LastCompletedIteration;
        dto.State = stateGrpc.State;
        dto.UpdatedAt = DateTime.ParseExact(
            stateGrpc.UpdatedAt,
            "MM/dd/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture
        );

        return dto;
    }

    private static SimulationOverviewDto ToDto(SimulationOverviewGrpc grpc)
    {
        var dto = new SimulationOverviewDto();

        dto.Id = Guid.Parse(grpc.Id);
        dto.CreatedDate = DateTime.ParseExact(
            grpc.CreatedDate,
            "MM/dd/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture
        );
        dto.SimulationParams = SimulationParamsToDto(grpc.SimulationParams);
        dto.LeagueStrengths = JsonConvert.DeserializeObject<List<LeagueStrengthDto>>(grpc.LeagueStrengths);
        dto.PriorLeagueStrength = grpc.PriorLeagueStrength;

        return dto;
    }

    private static SimulationParamsDto SimulationParamsToDto(SimulationParamsGrpc grpc)
    {
        var dto = new SimulationParamsDto();

        dto.Title = grpc.Title;
        dto.SeasonYears = grpc.SeasonYears.ToList();
        dto.Iterations = grpc.Iterations;
        dto.LeagueId = Guid.Parse(grpc.LeagueId);
        dto.Seed = grpc.Seed;
        dto.LeagueRoundId = grpc.HasLeagueRoundId ? Guid.Parse(grpc.LeagueRoundId) : Guid.Empty;
        dto.CreateScoreboardOnCompleteIteration = grpc.HasCreateScoreboardOnCompleteIteration;
        dto.GamesToReachTrust = grpc.GamesToReachTrust;
        dto.ConfidenceLevel = grpc.ConfidenceLevel;
        dto.HomeAdvantage = grpc.HomeAdvantage;
        dto.NoiseFactor = grpc.NoiseFactor;
        dto.TargetLeagueRoundId = Guid.Parse(grpc.TargetLeagueRoundId);
        dto.ModelType = grpc.Model;

        return dto;
    }

    private static SimulationParamsGrpc ToProto(SimulationParamsDto simulationParamsDto)
    {
        var grpc = new SimulationParamsGrpc();

        grpc.Title = simulationParamsDto.Title;
        grpc.LeagueId = simulationParamsDto.LeagueId.ToString();
        grpc.Iterations = simulationParamsDto.Iterations;
        grpc.Seed = simulationParamsDto.Seed;
        grpc.CreateScoreboardOnCompleteIteration = simulationParamsDto.CreateScoreboardOnCompleteIteration;
        grpc.GamesToReachTrust = simulationParamsDto.GamesToReachTrust;
        grpc.ConfidenceLevel = simulationParamsDto.ConfidenceLevel;
        grpc.HomeAdvantage = simulationParamsDto.HomeAdvantage;
        grpc.NoiseFactor = simulationParamsDto.NoiseFactor;
        grpc.TargetLeagueRoundId = simulationParamsDto.TargetLeagueRoundId.ToString();
        grpc.Model = simulationParamsDto.ModelType;

        grpc.SeasonYears.AddRange(simulationParamsDto.SeasonYears);

        if (simulationParamsDto.LeagueRoundId != null && simulationParamsDto.LeagueRoundId != Guid.Empty)
        {
            grpc.LeagueRoundId = simulationParamsDto.LeagueRoundId.ToString();
        }

        return grpc;
    }

    private static void Validate(SimulationParamsGrpc grpc)
    {
        string model = grpc.Model;

        if ("StandardPoisson" == model)
            return;
        if ("DixonColes" == model)
            return;
        if ("BivariatePoisson" == model)
            return;
        if ("Advanced" == model)
            return;

        throw new ArgumentException($"Invalid simulation model string type. Provided {model}");
    }
}
