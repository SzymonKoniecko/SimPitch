using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Interfaces;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

public class GetScoreboardsBySimulationIdQueryHandler : IRequestHandler<GetScoreboardsBySimulationIdQuery, List<ScoreboardDto>>
{
    private readonly IScoreboardReadRepository _scoreboardReadRepository;
    private readonly IMediator _mediator;
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public GetScoreboardsBySimulationIdQueryHandler
    (
        IScoreboardReadRepository scoreboardReadRepository,
        IMediator mediator,
        ISimulationEngineGrpcClient simulationEngineGrpcClient
    )
    {
        _scoreboardReadRepository = scoreboardReadRepository;
        _mediator = mediator;
        _simulationEngineGrpcClient = simulationEngineGrpcClient ?? throw new ArgumentNullException(nameof(simulationEngineGrpcClient));
    }

    public async Task<List<ScoreboardDto>> Handle(GetScoreboardsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        List<ScoreboardDto> scoreboardDtos = new List<ScoreboardDto>();
        var simulationOverview = await _simulationEngineGrpcClient.GetSimulationOverviewByIdAsync(query.simulationId, cancellationToken);
        if (simulationOverview == null)
            throw new KeyNotFoundException($"Missing simulation overview object for-> SimulationId:{query.simulationId}");

        if (await _scoreboardReadRepository.ScoreboardsBySimulationIdExistsAsync(query.simulationId, simulationOverview.SimulationParams.Iterations, cancellationToken: cancellationToken))
        {
            var scoreboards = await _scoreboardReadRepository.GetScoreboardBySimulationIdAsync(query.simulationId, withTeamStats: query.withTeamStats, cancellationToken: cancellationToken);

            foreach (var scoreboard in scoreboards)
                scoreboard.SortByRank();

            if (query.iterationResultId != Guid.Empty) // filter for requested iteration result
            {
                scoreboards = scoreboards.Where(x => x.IterationResultId == query.iterationResultId).ToList();
            }

            return scoreboards.Select(x => ScoreboardMapper.ToDto(x)).ToList();
        }
        else
        {
            var simulationState = await _simulationEngineGrpcClient.GetSimulationStateByIdAsync(query.simulationId, cancellationToken);
            if (simulationState.State != "Failed") // create a new MISSING scoreboards if simulationd is not failed
            {
                var command = new CreateScoreboardCommand(query.simulationId, Guid.Empty);
                await _mediator.Send(command, cancellationToken: cancellationToken);    
            }
        }
        var scoreboardsAfterAll = await _scoreboardReadRepository.GetScoreboardBySimulationIdAsync(query.simulationId, withTeamStats: query.withTeamStats, cancellationToken: cancellationToken);

        return scoreboardsAfterAll.Select(x => ScoreboardMapper.ToDto(x)).ToList();
    }
}