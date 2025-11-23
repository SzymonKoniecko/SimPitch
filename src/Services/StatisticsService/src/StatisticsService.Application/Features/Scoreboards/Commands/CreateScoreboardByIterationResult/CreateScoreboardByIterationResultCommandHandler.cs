using System;
using MediatR;
using Microsoft.Extensions.Logging;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByIterationResult;

public class CreateScoreboardByIterationResultCommandHandler : IRequestHandler<CreateScoreboardByIterationResultCommand, bool>
{
    private readonly IScoreboardDataService _scoreboardDataService;
    private readonly ILogger<CreateScoreboardByIterationResultCommandHandler> _logger;
    private readonly IScoreboardWriteRepository _scoreboardWriteRepository;
    private readonly IScoreboardReadRepository _scoreboardReadRepository;
    private readonly IScoreboardTeamStatsWriteRepository _scoreboardTeamStatsWriteRepository;
    private readonly ScoreboardService _scoreboardService;


    public CreateScoreboardByIterationResultCommandHandler
    (
        ILogger<CreateScoreboardByIterationResultCommandHandler> logger,
        IScoreboardWriteRepository scoreboardWriteRepository,
        IScoreboardReadRepository scoreboardReadRepository,
        ScoreboardService scoreboardService,
        IScoreboardTeamStatsWriteRepository scoreboardTeamStatsWriteRepository,
        IScoreboardDataService scoreboardDataService
    )
    {
        _logger = logger;
        _scoreboardWriteRepository = scoreboardWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardWriteRepository));
        _scoreboardReadRepository = scoreboardReadRepository;
        _scoreboardService = scoreboardService ?? throw new ArgumentNullException(nameof(scoreboardService));
        _scoreboardTeamStatsWriteRepository = scoreboardTeamStatsWriteRepository ?? throw new ArgumentNullException(nameof(scoreboardTeamStatsWriteRepository));
        _scoreboardDataService = scoreboardDataService;
    }

    public async Task<bool> Handle(CreateScoreboardByIterationResultCommand command, CancellationToken cancellationToken)
    {
        var playedMatchRounds = await _scoreboardDataService.GetPlayedMatchRoundsAsync(command.Overview, cancellationToken);

        var newScoreboard = await CreateMissingScoreboardAsync(IterationResultMapper.ToValueObject(command.iterationResultDto), playedMatchRounds, cancellationToken);
        
        if (newScoreboard == null)
            _logger.LogWarning($"Scoreboard calculation returned null value, check what happened-> IterationResultId:{command.iterationResultDto.Id}");


        bool existing = await _scoreboardReadRepository.ScoreboardByIterationResultIdExistsAsync(command.iterationResultDto.Id, cancellationToken);

        if (existing == false)
            _logger.LogWarning($"Cannot load created scoreboard-> IterationResultId:{command.iterationResultDto.Id}; <-data inconsistency detected.");

        return existing;
    }

    private async Task<Scoreboard> CreateMissingScoreboardAsync(
        IterationResult iterationResult,
        IEnumerable<MatchRoundDto> playedMatchRounds,
        CancellationToken ct
    )
    {

        if (await _scoreboardReadRepository.ScoreboardByIterationResultIdExistsAsync(iterationResult.Id, ct))
            throw new Exception($"Tried to create a scoreboard for already exisiting copy. IterationResultId:{iterationResult.Id}");
        
        var scoreboard = _scoreboardService.CalculateSingleScoreboard(
            iterationResult,
            playedMatchRounds.Select(MatchRoundMapper.ToValueObject).ToList());

        scoreboard.SetRankings();

        await _scoreboardWriteRepository.CreateScoreboardAsync(scoreboard, ct);
        await _scoreboardTeamStatsWriteRepository.CreateScoreboardTeamStatsBulkAsync(scoreboard.ScoreboardTeams, ct);

        return scoreboard;
    }
}
