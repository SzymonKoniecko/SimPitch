using System;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;

public class CreateScoreboardCommandHandler : IRequestHandler<CreateScoreboardCommand, List<ScoreboardDto>>
{
    private readonly IScoreboardWriteRepository _scoreboardWriteRepository;
    private readonly IMediator _mediator;
    private readonly ScoreboardService _scoreboardService;

    public CreateScoreboardCommandHandler(IScoreboardWriteRepository repository, IMediator mediator, ScoreboardService scoreboardService)
    {
        _scoreboardWriteRepository = repository;
        _mediator = mediator;
        _scoreboardService = scoreboardService;
    }

    public async Task<List<ScoreboardDto>> Handle(CreateScoreboardCommand request, CancellationToken cancellationToken)
    {
        var simulationResultsQuery = new GetSimulationResultsBySimulationIdQuery(request.simulationId);
        var simulationResults = await _mediator.Send(simulationResultsQuery, cancellationToken);

        var scoreboardList = new List<Scoreboard>();
        foreach (var simulationResult in simulationResults)
        {
            var scoreboard = _scoreboardService.CalculateSingleScoreboard(SimulationResultMapper.ToValueObject(simulationResult));
            scoreboardList.Add(scoreboard);

            await _scoreboardWriteRepository.CreateScoreboardAsync(scoreboard, cancellationToken: cancellationToken);
        }

        return (List<ScoreboardDto>)scoreboardList.Select(x => ScoreboardMapper.ToDto(x));
    }
}
