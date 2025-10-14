using System;
using EngineService.Application.DTOs;
using EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using EngineService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using EngineService.Application.Interfaces;
using EngineService.Application.Mappers;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetSimulationById;

public class GetSimulationByIdQueryHandler : IRequestHandler<GetSimulationByIdQuery, SimulationDto>
{
    private readonly IMediator _mediator;

    public GetSimulationByIdQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<SimulationDto> Handle(GetSimulationByIdQuery query, CancellationToken cancellationToken)
    {
        var iterationsQuery = new GetIterationResultsBySimulationIdQuery(query.simulationId);
        var scoreboardsQuery = new GetScoreboardsBySimulationIdQuery(query.simulationId, withTeamStats: true);

        List<IterationResultDto> iterationResults = await _mediator.Send(iterationsQuery, cancellationToken);
        List<ScoreboardDto> scoreboards = await _mediator.Send(scoreboardsQuery, cancellationToken);

        if (iterationResults == null || iterationResults.Count == 0)
        {
            return null;
        }

        List<IterationPreviewDto> iterationPreviewDtos = new();

        foreach (var scoreboard in scoreboards)
        {
            iterationPreviewDtos.AddRange(IterationPreviewMapper.GetIterationPreviewDtosAsync(scoreboard.ScoreboardTeams, iterationResults.First(x => x.Id == scoreboard.IterationResultId)));
        }

        return SimulationMapper.ToSimulationDto(
                query.simulationId,
                iterationResults.Count,
                iterationResults.First().SimulationParams,
                iterationPreviewDtos.OrderBy(x => x.Rank).ToList(),
                (int)(iterationResults.First()?.SimulatedMatchRounds.Count),
                (float)(iterationResults.First()?.PriorLeagueStrength)
            );
    }
}
