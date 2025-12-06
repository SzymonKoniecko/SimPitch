using System;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using EngineService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using EngineService.Application.Interfaces;
using EngineService.Application.Mappers;
using EngineService.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EngineService.Application.Features.Simulations.Queries.GetSimulationById;

public class GetSimulationByIdQueryHandler : IRequestHandler<GetSimulationByIdQuery, SimulationDto>
{
    private readonly IMediator _mediator;
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;
    private readonly ILogger<GetSimulationByIdQueryHandler> _logger;

    public GetSimulationByIdQueryHandler(
        IMediator mediator,
        ISimulationEngineGrpcClient simulationEngineGrpcClient,
        ILogger<GetSimulationByIdQueryHandler> logger)
    {
        _mediator = mediator;
        _simulationEngineGrpcClient = simulationEngineGrpcClient;
        _logger = logger;
    }
    public async Task<SimulationDto> Handle(GetSimulationByIdQuery query, CancellationToken cancellationToken)
    {

        var iterationsQuery = new GetIterationResultsBySimulationIdQuery(query.simulationId, query.PagedRequest);
        var simulationState = await _simulationEngineGrpcClient.GetSimulationStateAsync(query.simulationId, cancellationToken);
        if (simulationState.State != "Completed" || simulationState.State != "Failed")
        {
            Thread.Sleep(2000);
        }
        var simulationOverview = await _simulationEngineGrpcClient.GetSimulationOverviewBySimulationId(query.simulationId, cancellationToken);

        PagedResponse<IterationResultDto> iterationResults = await _mediator.Send(iterationsQuery, cancellationToken);

        if (iterationResults == null || iterationResults.Items.Count() == 0)
        {
            _logger.LogWarning($"No iteration results for query: Offset:{query.PagedRequest.Offset} - PageSize:{query.PagedRequest.PageSize}");
            return SimulationMapper.ToSimulationDto(
                query.simulationId,
                simulationState,
                simulationOverview.SimulationParams,
                new PagedResponse<IterationPreviewDto>()
                {
                    Items = null,
                    TotalCount = -1,
                    PageNumber = (query.PagedRequest.Offset / query.PagedRequest.PageSize) + 1,
                    PageSize = query.PagedRequest.PageSize,
                    SortingMethod = query.PagedRequest.SortingMethod
                },
                0,
                new List<LeagueStrengthDto>(),
                0
            );
        }
        if (simulationState == null)
            throw new KeyNotFoundException($"Not found simulation state, id:{simulationState}");

        List<IterationPreviewDto> iterationPreviewDtos = new();

        foreach (var iterationResult in iterationResults.Items)
        {
            var scoreboardsQuery = new GetScoreboardsBySimulationIdQuery(query.simulationId, iterationResult.Id, withTeamStats: true);
            List<ScoreboardDto> scoreboards = await _mediator.Send(scoreboardsQuery, cancellationToken);
            if (scoreboards == null || scoreboards.Count == 0)
                throw new KeyNotFoundException($"Missing scoreboard by iterationId {iterationResult.Id}");

            iterationPreviewDtos.AddRange(IterationPreviewMapper.GetIterationPreviewDtosAsync(scoreboards.First().ScoreboardTeams, iterationResult));
        }

        if (EnumMapper.SortingOptionToEnum(query.PagedRequest.SortingMethod.SortingOption) == SortingOptionEnum.LeaderPoints)
        {
            iterationPreviewDtos = iterationPreviewDtos.OrderBy(x => x.Points).ToList();
            if (query.PagedRequest.SortingMethod.Order == "DESC")
                iterationPreviewDtos.Reverse();
        }

        return SimulationMapper.ToSimulationDto(
                query.simulationId,
                simulationState,
                simulationOverview.SimulationParams,
                new PagedResponse<IterationPreviewDto>()
                {
                    Items = iterationPreviewDtos,
                    TotalCount = iterationResults.TotalCount,
                    PageNumber = iterationResults.PageNumber,
                    PageSize = iterationResults.PageSize,
                    SortingMethod = query.PagedRequest.SortingMethod
                },
                (int)(iterationResults.Items.First()?.SimulatedMatchRounds.Count),
                simulationOverview.LeagueStrengths,
                simulationOverview.PriorLeagueStrength
            );
    }
}
