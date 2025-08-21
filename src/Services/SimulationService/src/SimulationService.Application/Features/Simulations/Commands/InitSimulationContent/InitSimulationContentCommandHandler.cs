using MediatR;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.Leagues.Query.GetLeagueById;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Enums;
using SimulationService.Application.Features.LeagueRounds.DTOs;

public partial class InitSimulationContentCommandHandler : IRequestHandler<InitSimulationContentCommand, InitSimulationContentResponse>
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

        var leagueRoundDtoRequest = new LeagueRoundDtoRequest
        {
            SeasonYear = query.SimulationParamsDto.SeasonYear,
            LeagueRoundId = query.SimulationParamsDto.RoundId
        };

        var leagueRounds = await _mediator.Send(new GetLeagueRoundsByParamsGrpcQuery(leagueRoundDtoRequest));

        int totalGoals = 0;
        int totalMatches = 0;
        League league = null;

        foreach (var leagueRound in leagueRounds)
        {
            if (league == null || league.Id != leagueRound.LeagueId)
            {
                league = await _mediator.Send(new GetLeagueByIdQuery(leagueRound.LeagueId));
                contentResponse.LeagueStrength = league.Strength;
            }

            var matchRounds = await _mediator.Send(new GetMatchRoundsByIdQuery(leagueRound.Id));
            contentResponse.MatchRoundsToSimulate.AddRange(matchRounds.Where(m => !m.IsPlayed));

            contentResponse = CalculateSeasonStatsByMatchRounds(
                contentResponse,
                matchRounds,
                leagueRound,
                ref totalGoals,
                ref totalMatches
            );
        }

        contentResponse.PriorLeagueStrength = totalMatches > 0 ? (float)totalGoals / totalMatches : 0f;

        contentResponse.TeamsStrengthDictionary = contentResponse.TeamsStrengthDictionary
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.WithLikelihood().WithPosterior(contentResponse.PriorLeagueStrength)
            );

        return contentResponse;
    }

    private InitSimulationContentResponse CalculateSeasonStatsByMatchRounds(
        InitSimulationContentResponse contentResponse,
        IEnumerable<MatchRound> matchRounds,
        LeagueRound leagueRound,
        ref int totalGoals,
        ref int totalMatches)
    {
        var seasonEnum = EnumMapper.StringtoSeasonEnum(leagueRound.SeasonYear);

        foreach (var matchRound in matchRounds)
        {
            UpdateTeamStats(contentResponse, matchRound.HomeTeamId, matchRound, seasonEnum, leagueRound.LeagueId, true);
            UpdateTeamStats(contentResponse, matchRound.AwayTeamId, matchRound, seasonEnum, leagueRound.LeagueId, false);

            totalGoals += matchRound.HomeGoals + matchRound.AwayGoals;
            totalMatches += 2;
        }

        return contentResponse;
    }

    private void UpdateTeamStats(
        InitSimulationContentResponse response,
        Guid teamId,
        MatchRound matchRound,
        SeasonEnum seasonEnum,
        Guid leagueId,
        bool isHomeTeam)
    {
        if (response.TeamsStrengthDictionary.TryGetValue(teamId, out var existingTeamStrength))
        {
            var updatedStats = _seasonStatsService.CalculateSeasonStats(matchRound, existingTeamStrength.SeasonStats, seasonEnum, leagueId, isHomeTeam);
            response.TeamsStrengthDictionary[teamId] = existingTeamStrength with { SeasonStats = updatedStats };
        }
        else
        {
            var newTeamStrength = TeamStrength.Create(teamId, seasonEnum, leagueId);
            var updatedStats = _seasonStatsService.CalculateSeasonStats(matchRound, newTeamStrength.SeasonStats, seasonEnum, leagueId, isHomeTeam);
            newTeamStrength = newTeamStrength with { SeasonStats = updatedStats };
            response.TeamsStrengthDictionary.Add(teamId, newTeamStrength);
        }
    }
}
