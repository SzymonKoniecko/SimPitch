using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

public class GetScoreboardsBySimulationIdQueryHandler : IRequestHandler<GetScoreboardsBySimulationIdQuery, List<ScoreboardDto>>
{
    private readonly IScoreboardReadRepository _scoreboardReadRepository;
    private readonly IMediator _mediator;

    public GetScoreboardsBySimulationIdQueryHandler(IScoreboardReadRepository scoreboardReadRepository, IMediator mediator)
    {
        _scoreboardReadRepository = scoreboardReadRepository;
        this._mediator = mediator;
    }

    public async Task<List<ScoreboardDto>> Handle(GetScoreboardsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        List<ScoreboardDto> scoreboardDtos = new List<ScoreboardDto>();

        if (await _scoreboardReadRepository.ScoreboardBySimulationIdExistsAsync(query.simulationId, cancellationToken: cancellationToken))
        {
            var scoreboards = await _scoreboardReadRepository.GetScoreboardBySimulationIdAsync(query.simulationId, withTeamStats: query.withTeamStats, cancellationToken: cancellationToken);
            
            foreach (var scoreboard in scoreboards)
                scoreboard.SortByRank();
                
            if (query.iterationResultId != Guid.Empty)
            {
                scoreboards = scoreboards.Where(x => x.IterationResultId == query.iterationResultId).ToList();
            }

            return scoreboards.Select(x => ScoreboardMapper.ToDto(x)).ToList();
        }
        return null;
    }
}
