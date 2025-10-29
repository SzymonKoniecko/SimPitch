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
using StatisticsService.Domain.ValueObjects;

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
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public CreateScoreboardCommandHandler(
        IScoreboardWriteRepository scoreboardWriteRepository,
        IMediator mediator,
        ScoreboardService scoreboardService,
        IScoreboardTeamStatsWriteRepository scoreboardTeamStatsWriteRepository,
        IScoreboardReadRepository scoreboardReadRepository,
        ILogger<CreateScoreboardCommandHandler> logger,
        ILeagueRoundGrpcClient leagueRoundGrpcClient,
        IMatchRoundGrpcClient matchRoundGrpcClient,
        ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _scoreboardWriteRepository = scoreboardWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardWriteRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _scoreboardService = scoreboardService ?? throw new ArgumentNullException(nameof(scoreboardService));
        _scoreboardTeamStatsWriteRepository = scoreboardTeamStatsWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardTeamStatsWriteRepository));
        _scoreboardReadRepository = scoreboardReadRepository ?? throw new ArgumentNullException(nameof(scoreboardReadRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _leagueRoundGrpcClient = leagueRoundGrpcClient ?? throw new ArgumentNullException(nameof(leagueRoundGrpcClient));
        _matchRoundGrpcClient = matchRoundGrpcClient ?? throw new ArgumentNullException(nameof(matchRoundGrpcClient));
        _simulationEngineGrpcClient = simulationEngineGrpcClient ?? throw new ArgumentNullException(nameof(simulationEngineGrpcClient));
    }

    public async Task<IEnumerable<ScoreboardDto>> Handle(CreateScoreboardCommand request, CancellationToken cancellationToken)
    {
        var iterationResults = await GetIterationResultsAsync(request, cancellationToken);
        var simulationOverview = await _simulationEngineGrpcClient.GetSimulationOverviewByIdAsync(request.simulationId, cancellationToken);

        var playedMatchRounds = await GetPlayedMatchRoundsAsync(simulationOverview, cancellationToken);

        if (request.iterationResultId != Guid.Empty)
        {
            iterationResults = iterationResults
                .Where(x => x.Id == request.iterationResultId)
                .ToList();
        }

        var newScoreboards = await CreateMissingScoreboardsAsync(iterationResults, playedMatchRounds, cancellationToken);

        if (newScoreboards.Any())
            return newScoreboards.Select(ScoreboardMapper.ToDto);

        var existing = await _scoreboardReadRepository.GetScoreboardBySimulationIdAsync(
            request.simulationId, withTeamStats: true, cancellationToken);

        if (existing == null)
            throw new InvalidOperationException("Cannot load created scoreboards; data inconsistency detected.");

        var existingDto = existing.Select(x => ScoreboardMapper.ToDto(x));
        return request.iterationResultId != Guid.Empty
            ? existingDto.Where(x => x.IterationResultId == request.iterationResultId)
            : existingDto;
    }

    private async Task<IEnumerable<IterationResult>> GetIterationResultsAsync(CreateScoreboardCommand request, CancellationToken ct)
    {
        var query = new GetIterationResultsBySimulationIdQuery(request.simulationId);
        var results = await _mediator.Send(query, ct);

        if (results == null || !results.Any())
            throw new KeyNotFoundException("No simulation results found for the given simulation ID.");

        return results.Select(x => IterationResultMapper.ToValueObject(x));
    }

    private async Task<List<MatchRoundDto>> GetPlayedMatchRoundsAsync(SimulationOverview overview, CancellationToken ct)
    {
        var leagueRoundRequest = new LeagueRoundDtoRequest
        {
            LeagueId = overview.SimulationParams.LeagueId,
            SeasonYears = overview.SimulationParams.SeasonYears,
            LeagueRoundId = overview.SimulationParams.LeagueRoundId
        };

        var leagueRounds = await _leagueRoundGrpcClient.GetAllLeagueRoundsByParams(leagueRoundRequest, ct);

        if (leagueRounds == null || leagueRounds.Count == 0)
            throw new InvalidOperationException("No league rounds found for the given parameters.");

        var playedRounds = new List<MatchRoundDto>();

        foreach (var round in leagueRounds)
        {
            var matches = await _matchRoundGrpcClient.GetMatchRoundsByRoundIdAsync(round.Id, ct);
            if (matches != null)
                playedRounds.AddRange(matches.Where(x => x.IsPlayed));
        }

        return playedRounds;
    }

    private async Task<List<Scoreboard>> CreateMissingScoreboardsAsync(
        IEnumerable<IterationResult> iterationResults,
        IEnumerable<MatchRoundDto> playedMatchRounds,
        CancellationToken ct)
    {
        var scoreboards = new List<Scoreboard>();

        foreach (var result in iterationResults)
        {
            if (await _scoreboardReadRepository.ScoreboardByIterationResultIdExistsAsync(result.Id, ct))
                continue;

            var scoreboard = _scoreboardService.CalculateSingleScoreboard(
                result,
                playedMatchRounds.Select(MatchRoundMapper.ToValueObject).ToList());

            scoreboard.SetRankings();
            scoreboards.Add(scoreboard);

            await _scoreboardWriteRepository.CreateScoreboardAsync(scoreboard, ct);
            await _scoreboardTeamStatsWriteRepository.CreateScoreboardTeamStatsBulkAsync(scoreboard.ScoreboardTeams, ct);
        }

        return scoreboards;
    }
}
