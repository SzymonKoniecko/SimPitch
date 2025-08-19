using System;
using MediatR;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.Leagues.Query.GetLeagueById;
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
        League league = new();
        LeagueRoundDtoRequest leagueRoundDtoRequest = new();
        leagueRoundDtoRequest.SeasonYear = query.SimulationParamsDto.SeasonYear;
        leagueRoundDtoRequest.LeagueRoundId = query.SimulationParamsDto.RoundId;

        var leagueRounds = await _mediator.Send(new GetLeagueRoundsByParamsGrpcQuery(leagueRoundDtoRequest));

        int totalGoals = 0;
        int totalMatches = 0;
        foreach (var leagueRound in leagueRounds)
        {
            if (league == null || league.Id != leagueRound.LeagueId)
            {
                league = await _mediator.Send(new GetLeagueByIdQuery(leagueRound.LeagueId));
                contentResponse.LeagueStrength = league.Strength;
            }

            var response = await _mediator.Send(new GetMatchRoundsByIdQuery(leagueRound.Id));
            contentResponse.MatchRoundsToSimulate.AddRange(response.Where(mr => mr.IsPlayed == false));

            foreach (var matchRound in response.Where(mr => mr.IsPlayed == true))
            {
                // tutaj zliczaj prior
                if (contentResponse.TeamsStrengthDictionary.ContainsKey(matchRound.HomeTeamId))
                {
                    contentResponse.TeamsStrengthDictionary[matchRound.HomeTeamId].SeasonStats = _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, contentResponse.TeamsStrengthDictionary.GetValueOrDefault(matchRound.HomeTeamId)?.SeasonStats, true);
                }
                if (contentResponse.TeamsStrengthDictionary.ContainsKey(matchRound.AwayTeamId))
                {
                    contentResponse.TeamsStrengthDictionary[matchRound.AwayTeamId].SeasonStats = _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, contentResponse.TeamsStrengthDictionary.GetValueOrDefault(matchRound.AwayTeamId)?.SeasonStats, false);
                }
                if (!contentResponse.TeamsStrengthDictionary.ContainsKey(matchRound.HomeTeamId))
                {
                    TeamStrength teamStrength = new();
                    teamStrength.TeamId = matchRound.HomeTeamId;
                    teamStrength.SeasonStats = new();
                    teamStrength.SeasonStats.Id = Guid.NewGuid();
                    teamStrength.SeasonStats.TeamId = matchRound.HomeTeamId;
                    teamStrength.SeasonStats.SeasonYear = EnumMapper.StringtoSeasonEnum(leagueRoundDtoRequest.SeasonYear);
                    teamStrength.SeasonStats = _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, teamStrength.SeasonStats, true);
                    contentResponse.TeamsStrengthDictionary.Add(matchRound.HomeTeamId, teamStrength);
                }

                if (!contentResponse.TeamsStrengthDictionary.ContainsKey(matchRound.AwayTeamId))
                {
                    TeamStrength teamStrength = new();
                    teamStrength.TeamId = matchRound.AwayTeamId;
                    teamStrength.SeasonStats = new();
                    teamStrength.SeasonStats.Id = Guid.NewGuid();
                    teamStrength.SeasonStats.TeamId = matchRound.AwayTeamId;
                    teamStrength.SeasonStats.SeasonYear = EnumMapper.StringtoSeasonEnum(leagueRoundDtoRequest.SeasonYear);
                    teamStrength.SeasonStats = _seasonStatsService.CalculateSeasonStatsForCurrentSeasonAsync(matchRound, teamStrength.SeasonStats, true);
                    contentResponse.TeamsStrengthDictionary.Add(matchRound.AwayTeamId, teamStrength);
                }
                totalGoals += matchRound.HomeGoals + matchRound.AwayGoals;
                totalMatches += 2; // Each match round has two teams, so we count it as two matches
            }
        }
        if (totalMatches > 0)
            contentResponse.PriorLeagueStrength = (float)totalGoals / totalMatches;
        else
            contentResponse.PriorLeagueStrength = 0;

        return contentResponse;
    }
    
    public async Task<object> CalculateSeasonStatsForSimulatedMatchRoundsAsync(Guid leagueId, int seasonYear, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This method is not implemented yet. Please implement the logic to calculate season stats for simulated match rounds.");
    } 
}
