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
    private readonly IScoreboardWriteRepository _scoreboardWriteRepository;
    private readonly IScoreboardTeamStatsWriteRepository _scoreboardTeamStatsWriteRepository;
    private readonly IScoreboardReadRepository _scoreboardReadRepository;
    private readonly ScoreboardService _scoreboardService;
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;
    private readonly IScoreboardDataService _scoreboardDataService;

    public CreateScoreboardCommandHandler
    (
        IScoreboardWriteRepository scoreboardWriteRepository,
        ScoreboardService scoreboardService,
        IScoreboardTeamStatsWriteRepository scoreboardTeamStatsWriteRepository,
        IScoreboardReadRepository scoreboardReadRepository,
        ISimulationEngineGrpcClient simulationEngineGrpcClient,
        IScoreboardDataService scoreboardDataService
    )
    {
        _scoreboardWriteRepository = scoreboardWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardWriteRepository));
        _scoreboardService = scoreboardService ?? throw new ArgumentNullException(nameof(scoreboardService));
        _scoreboardTeamStatsWriteRepository = scoreboardTeamStatsWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardTeamStatsWriteRepository));
        _scoreboardReadRepository = scoreboardReadRepository ?? throw new ArgumentNullException(nameof(scoreboardReadRepository));
        _simulationEngineGrpcClient = simulationEngineGrpcClient ?? throw new ArgumentNullException(nameof(simulationEngineGrpcClient));
        _scoreboardDataService = scoreboardDataService;
    }

    public async Task<IEnumerable<ScoreboardDto>> Handle(CreateScoreboardCommand request, CancellationToken cancellationToken)
    {
        var iterationResults = await _scoreboardDataService.GetIterationResultsAsync(request.simulationId, cancellationToken);
        var simulationOverview = await _simulationEngineGrpcClient.GetSimulationOverviewByIdAsync(request.simulationId, cancellationToken);

        var playedMatchRounds = await _scoreboardDataService.GetPlayedMatchRoundsAsync(simulationOverview, cancellationToken);

        if (request.iterationResultId != Guid.Empty)
        {
            iterationResults = iterationResults
                .Where(x => x.Id == request.iterationResultId)
                .ToList();
        }

        var newScoreboards = await CreateMissingScoreboardsAsync(simulationOverview.SimulationParams, iterationResults, playedMatchRounds, cancellationToken);

        if (newScoreboards.Any())
            return newScoreboards.Select(ScoreboardMapper.ToDto);

        var existing = await _scoreboardReadRepository.GetScoreboardByQueryAsync(
            request.simulationId, request.iterationResultId, withTeamStats: true, cancellationToken);

        if (existing == null)
            throw new InvalidOperationException("Cannot load created scoreboards; data inconsistency detected.");

        var existingDto = existing.Select(x => ScoreboardMapper.ToDto(x));
        return request.iterationResultId != Guid.Empty
            ? existingDto.Where(x => x.IterationResultId == request.iterationResultId)
            : existingDto;
    }


    private async Task<List<Scoreboard>> CreateMissingScoreboardsAsync
    (
        SimulationParams simulationParams,
        IEnumerable<IterationResult> iterationResults,
        IEnumerable<MatchRoundDto> playedMatchRounds,
        CancellationToken ct
    )
    {
        var scoreboards = new List<Scoreboard>();

        foreach (var result in iterationResults)
        {
            if (await _scoreboardReadRepository.ScoreboardByIterationResultIdExistsAsync(result.Id, ct))
                continue;

            var scoreboard = _scoreboardService.CalculateSingleScoreboard(
                simulationParams,
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
