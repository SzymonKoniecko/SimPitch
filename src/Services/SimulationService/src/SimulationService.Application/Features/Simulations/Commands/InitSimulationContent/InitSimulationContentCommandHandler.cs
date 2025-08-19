using System;
using MediatR;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;

public class InitSimulationContentCommandHandler : IRequestHandler<InitSimulationContentCommand, InitSimulationContentResponse>
{
    private readonly SeasonStatsService _seasonStatsService;
    private readonly IMediator _mediator;
    

    public InitSimulationContentCommandHandler(SeasonStatsService seasonStatsService, IMediator mediator)
    {
        _seasonStatsService = seasonStatsService ?? throw new ArgumentNullException(nameof(seasonStatsService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<InitSimulationContentResponse> Handle(InitSimulationContentCommand query, CancellationToken cancellationToken)
    {
        InitSimulationContentResponse contentResponse = new();

        LeagueRoundDtoRequest leagueRoundDtoRequest = new();
        leagueRoundDtoRequest.SeasonYear = query.SimulationParamsDto.SeasonYear;
        leagueRoundDtoRequest.LeagueRoundId = query.SimulationParamsDto.RoundId;

        var leagueRounds = await _mediator.Send(new GetLeagueRoundsByParamsGrpcQuery(leagueRoundDtoRequest));

        foreach (var leagueRound in leagueRounds)
        {
            var response = await _mediator.Send(new GetMatchRoundsByIdQuery(leagueRound.Id));

            contentResponse.MatchRoundsToSimulate.AddRange(response.Where(mr => mr.IsPlayed == false));
            
            foreach (var matchRound in response.Where(mr => mr.IsPlayed == true))
            {
                if (contentResponse.SeasonStatsDictionary.ContainsKey(matchRound.HomeTeamId))
                {
                    contentResponse.SeasonStatsDictionary[matchRound.HomeTeamId] = _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, contentResponse.SeasonStatsDictionary.GetValueOrDefault(matchRound.HomeTeamId), true);
                }
                if (contentResponse.SeasonStatsDictionary.ContainsKey(matchRound.AwayTeamId))
                {
                    contentResponse.SeasonStatsDictionary[matchRound.AwayTeamId] = _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, contentResponse.SeasonStatsDictionary.GetValueOrDefault(matchRound.AwayTeamId), false);
                }
                if (!contentResponse.SeasonStatsDictionary.ContainsKey(matchRound.HomeTeamId))
                {
                    SeasonStats teamSeasonStats = new();
                    teamSeasonStats.Id = Guid.NewGuid();
                    teamSeasonStats.TeamId = matchRound.HomeTeamId;
                    teamSeasonStats.SeasonYear = EnumMapper.StringtoSeasonEnum(leagueRoundDtoRequest.SeasonYear);

                    contentResponse.SeasonStatsDictionary.Add(matchRound.HomeTeamId, _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, teamSeasonStats, true));
                }

                if (!contentResponse.SeasonStatsDictionary.ContainsKey(matchRound.AwayTeamId))
                {
                    SeasonStats teamSeasonStats = new();
                    teamSeasonStats.Id = Guid.NewGuid();
                    teamSeasonStats.TeamId = matchRound.AwayTeamId;
                    teamSeasonStats.SeasonYear = EnumMapper.StringtoSeasonEnum(leagueRoundDtoRequest.SeasonYear);

                    contentResponse.SeasonStatsDictionary.Add(matchRound.AwayTeamId, _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, teamSeasonStats, false));
                }
            }
        }
        return contentResponse;
    }
}
