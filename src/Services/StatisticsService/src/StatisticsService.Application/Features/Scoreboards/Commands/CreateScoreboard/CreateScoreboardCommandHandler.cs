using System;
using MediatR;
using Microsoft.Extensions.Logging;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.LeagueRounds.DTOs;
using StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using StatisticsService.Application.Interfaces;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;
using StatisticsService.Application.DTOs.Clients;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;

public class CreateScoreboardCommandHandler : IRequestHandler<CreateScoreboardCommand, IEnumerable<ScoreboardDto>>
{
    private readonly ILogger<CreateScoreboardCommandHandler> _logger;
    private readonly IScoreboardWriteRepository _scoreboardWriteRepository;
    private readonly IScoreboardTeamStatsWriteRepository _scoreboardTeamStatsWriteRepository;
    private readonly IScoreboardReadRepository _scoreboardReadRepository;
    private readonly IMediator _mediator;
    private readonly ScoreboardService _scoreboardService;
    private readonly ILeagueRoundGrpcClient _leagueRoundGrpcClient;
    private readonly IMatchRoundGrpcClient _matchRoundGrpcClient;
    private readonly ISimulationEngineGrpcClient simulationEngineGrpcClient;

    public CreateScoreboardCommandHandler(
        IScoreboardWriteRepository repository,
        IMediator mediator,
        ScoreboardService scoreboardService,
        IScoreboardTeamStatsWriteRepository scoreboardTeamStatsWriteRepository,
        IScoreboardReadRepository scoreboardReadRepository,
        ILogger<CreateScoreboardCommandHandler> logger,
        ILeagueRoundGrpcClient leagueRoundGrpcClient,
        IMatchRoundGrpcClient matchRoundGrpcClient,
        ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _scoreboardWriteRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _scoreboardService = scoreboardService ?? throw new ArgumentNullException(nameof(scoreboardService));
        _scoreboardTeamStatsWriteRepository = scoreboardTeamStatsWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardTeamStatsWriteRepository));
        _scoreboardReadRepository = scoreboardReadRepository ?? throw new ArgumentNullException(nameof(scoreboardReadRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._leagueRoundGrpcClient = leagueRoundGrpcClient ?? throw new ArgumentNullException(nameof(leagueRoundGrpcClient));
        this._matchRoundGrpcClient = matchRoundGrpcClient ?? throw new ArgumentNullException(nameof(matchRoundGrpcClient));
        this.simulationEngineGrpcClient = simulationEngineGrpcClient ?? throw new ArgumentNullException(nameof(simulationEngineGrpcClient));
    }

    public async Task<IEnumerable<ScoreboardDto>> Handle(CreateScoreboardCommand request, CancellationToken cancellationToken)
    {
        var IterationResultQuery = new GetIterationResultsBySimulationIdQuery(request.simulationId);
        var IterationResults = await _mediator.Send(IterationResultQuery, cancellationToken);
        SimulationOverview simulationOverview = await simulationEngineGrpcClient.GetSimulationOverviewByIdAsync(request.simulationId, cancellationToken);

        if (IterationResults == null || !IterationResults.Any())
        {
            throw new Exception("No simulation results found for the given simulation ID");
        }

        var leagueRoundRequest = new LeagueRoundDtoRequest
        {
            LeagueId = simulationOverview.SimulationParams.LeagueId,
            SeasonYears = simulationOverview.SimulationParams.SeasonYears,
            LeagueRoundId = simulationOverview.SimulationParams.LeagueRoundId
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

        if (request.iterationResultId != Guid.Empty)
        {
            IterationResults = IterationResults.Where(x => x.Id == request.iterationResultId).ToList();
        }
        if (IterationResults == null || IterationResults.Count == 0)
        {
            throw new Exception("No simulation results found for the given simulation ID");
        }
        
        
        var scoreboardList = new List<Scoreboard>();
        foreach (var iterationResult in IterationResults)
        {
            if (await _scoreboardReadRepository.ScoreboardByIterationResultIdExistsAsync(iterationResult.Id, cancellationToken: cancellationToken) == false) // we don't need to create the scoreboard again, and again
            {
                var scoreboard = _scoreboardService.CalculateSingleScoreboard(
                    IterationResultMapper.ToValueObject(iterationResult),
                    playedMatchRounds.Select(x => MatchRoundMapper.ToValueObject(x)).ToList()
                );
                scoreboard.SetRankings();
                scoreboardList.Add(scoreboard);

                await _scoreboardWriteRepository.CreateScoreboardAsync(scoreboard, cancellationToken: cancellationToken);
                await _scoreboardTeamStatsWriteRepository.CreateScoreboardTeamStatsBulkAsync(scoreboard.ScoreboardTeams, cancellationToken);
            }
        }

        return scoreboardList.Count > 0 ? scoreboardList.Select(x => ScoreboardMapper.ToDto(x)) : throw new Exception("Failed to create scoreboard");
    }
}
