using System;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using StatisticsService.Application.Features.LeagueRounds.DTOs;
using StatisticsService.Application.Interfaces;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Features.Scoreboards.Services;

public class ScoreboardDataService : IScoreboardDataService
{
    private readonly IMediator _mediator;
    private readonly ILeagueRoundGrpcClient _leagueRoundGrpcClient;
    private readonly IMatchRoundGrpcClient _matchRoundGrpcClient;

    public ScoreboardDataService
    (
        IMediator mediator,
        ILeagueRoundGrpcClient leagueRoundGrpcClient,
        IMatchRoundGrpcClient matchRoundGrpcClient
    )
    {
        _mediator = mediator;
        _leagueRoundGrpcClient = leagueRoundGrpcClient;
        _matchRoundGrpcClient = matchRoundGrpcClient;
    }

    public async Task<IEnumerable<IterationResult>> GetIterationResultsAsync(Guid simulationId, CancellationToken ct)
    {
        var query = new GetIterationResultsBySimulationIdQuery(simulationId);
        var results = await _mediator.Send(query, ct);

        if (results == null || !results.Any())
            throw new KeyNotFoundException("No simulation results found for the given simulation ID.");

        return results.Select(x => IterationResultMapper.ToValueObject(x));
    }

    public async Task<List<MatchRoundDto>> GetPlayedMatchRoundsAsync(SimulationOverview overview, CancellationToken ct)
    {
        var leagueRoundRequest = new LeagueRoundDtoRequest
        {
            LeagueId = overview.SimulationParams.LeagueId,
            SeasonYears = overview.SimulationParams.SeasonYears,
            
            // no needs to use - custom league round id as start of simulation 
            // (isPlayed = true, are marked as false in IterationResult)
            //LeagueRoundId = overview.SimulationParams.LeagueRoundId 
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
}
