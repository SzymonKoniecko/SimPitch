using System;
using MediatR;
using Microsoft.Extensions.Logging;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.LeagueRounds.DTOs;
using StatisticsService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;
using StatisticsService.Application.Interfaces;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;

public class CreateScoreboardCommandHandler : IRequestHandler<CreateScoreboardCommand, IEnumerable<ScoreboardDto>>
{
    private readonly ILogger<CreateScoreboardCommandHandler> _logger;
    private readonly IScoreboardWriteRepository _scoreboardWriteRepository;
    private readonly IScoreboardTeamStatsWriteRepository _scoreboardTeamStatsWriteRepository;
    private readonly IMediator _mediator;
    private readonly ScoreboardService _scoreboardService;
    private readonly ILeagueRoundGrpcClient _leagueRoundGrpcClient;
    private readonly IMatchRoundGrpcClient _matchRoundGrpcClient;

    public CreateScoreboardCommandHandler(IScoreboardWriteRepository repository, IMediator mediator, ScoreboardService scoreboardService, IScoreboardTeamStatsWriteRepository scoreboardTeamStatsWriteRepository, ILogger<CreateScoreboardCommandHandler> logger, ILeagueRoundGrpcClient leagueRoundGrpcClient, IMatchRoundGrpcClient matchRoundGrpcClient)
    {
        _scoreboardWriteRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _scoreboardService = scoreboardService ?? throw new ArgumentNullException(nameof(scoreboardService));
        _scoreboardTeamStatsWriteRepository = scoreboardTeamStatsWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardTeamStatsWriteRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._leagueRoundGrpcClient = leagueRoundGrpcClient ?? throw new ArgumentNullException(nameof(leagueRoundGrpcClient));
        this._matchRoundGrpcClient = matchRoundGrpcClient ?? throw new ArgumentNullException(nameof(matchRoundGrpcClient));
    }

    public async Task<IEnumerable<ScoreboardDto>> Handle(CreateScoreboardCommand request, CancellationToken cancellationToken)
    {
        var simulationResultsQuery = new GetSimulationResultsBySimulationIdQuery(request.simulationId);
        var simulationResults = await _mediator.Send(simulationResultsQuery, cancellationToken);
        
        if (simulationResults == null || !simulationResults.Any())
        {
            throw new Exception("No simulation results found for the given simulation ID");
        }

        var firstResult = simulationResults.First(); // needs to be corrected #25
        var leagueRoundRequest = new LeagueRoundDtoRequest
        {
            LeagueId = firstResult.SimulationParams.LeagueId,
            SeasonYears = firstResult.SimulationParams.SeasonYears,
            LeagueRoundId = firstResult.SimulationParams.LeagueRoundId
        };

        var leagueRounds = await _leagueRoundGrpcClient.GetAllLeagueRoundsByParams(leagueRoundRequest, cancellationToken);
        if (leagueRounds == null || leagueRounds.Count == 0)
        {
            throw new Exception("No league rounds found for the given parameters");
        }
        List<MatchRoundDto> playedMatchRounds = new List<MatchRoundDto>();
        foreach (var lr in leagueRounds)
        {
            var matchRounds = await _matchRoundGrpcClient.GetMatchRoundsByRoundIdAsync(lr.Id, cancellationToken);
            if (matchRounds != null && matchRounds.Count > 0)
            {
                playedMatchRounds.AddRange(matchRounds.Where(x => x.IsPlayed));
            }
        }

        if (request.simulationResultId != Guid.Empty)
        {
            simulationResults = simulationResults.Where(x => x.Id == request.simulationResultId).ToList();
        }
        if (simulationResults == null || simulationResults.Count == 0)
        {
            throw new Exception("No simulation results found for the given simulation ID");
        }
        
        var scoreboardList = new List<Scoreboard>();
        foreach (var simulationResult in simulationResults)
        {
            var scoreboard = _scoreboardService.CalculateSingleScoreboard(
                SimulationResultMapper.ToValueObject(simulationResult),
                playedMatchRounds.Select(x => MatchRoundMapper.ToValueObject(x)).ToList()
            );
            scoreboard.SetRankings();
            scoreboardList.Add(scoreboard);

            await _scoreboardWriteRepository.CreateScoreboardAsync(scoreboard, cancellationToken: cancellationToken);
            await _scoreboardTeamStatsWriteRepository.CreateScoreboardTeamStatsBulkAsync(scoreboard.ScoreboardTeams, cancellationToken);
        }

        return scoreboardList.Count > 0 ? scoreboardList.Select(x => ScoreboardMapper.ToDto(x)) : throw new Exception("Failed to create scoreboard");
    }
}
