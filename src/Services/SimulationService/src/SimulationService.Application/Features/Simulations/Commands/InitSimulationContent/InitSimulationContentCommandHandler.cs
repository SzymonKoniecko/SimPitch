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
using SimulationService.Application.Interfaces;
using SimulationService.Application.Features.SeasonsStats.Queries.GetSeasonsStatsByTeamIdGrpc;

public partial class InitSimulationContentCommandHandler : IRequestHandler<InitSimulationContentCommand, SimulationContent>
{
    private readonly SeasonStatsService _seasonStatsService;
    private readonly IMediator _mediator;

    public InitSimulationContentCommandHandler(SeasonStatsService seasonStatsService, IMediator mediator)
    {
        _seasonStatsService = seasonStatsService ?? throw new ArgumentNullException(nameof(seasonStatsService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<SimulationContent> Handle(InitSimulationContentCommand query, CancellationToken cancellationToken)
    {
        SimulationContent contentResponse = new();
        contentResponse.SimulationParams = new SimulationParams();
        contentResponse.SimulationParams.SeasonYears = query.SimulationParamsDto.SeasonYears.ToList();
        contentResponse.SimulationParams.LeagueId = query.SimulationParamsDto.LeagueId;
        contentResponse.SimulationParams.Iterations = query.SimulationParamsDto.Iterations;
        contentResponse.SimulationParams.LeagueRoundId = query.SimulationParamsDto.LeagueRoundId;

        var leagueRoundDtoRequest = new LeagueRoundDtoRequest
        {
            SeasonYears = query.SimulationParamsDto.SeasonYears,
            LeagueId = query.SimulationParamsDto.LeagueId,
            LeagueRoundId = query.SimulationParamsDto.LeagueRoundId,
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
                league.Strength,
                ref totalGoals,
                ref totalMatches
            );
        }

        if (query.SimulationParamsDto.SeasonYears.Count() > 1)
        {
            foreach (var (key, value) in contentResponse.TeamsStrengthDictionary)
            {
                var seasonStatsList = await _mediator.Send(new GetSeasonsStatsByTeamIdGrpcQuery(key));

                foreach (var singleSeasonStats in seasonStatsList)
                {
                    if (query.SimulationParamsDto.SeasonYears.Contains(EnumMapper.SeasonEnumToString(singleSeasonStats.SeasonYear)) == false)
                    {
                        continue;
                    }
                    if (singleSeasonStats.LeagueStrength != 1)
                    {
                        int x = 0;
                    }
                    var merged = value.SeasonStats.Merge(
                        value.SeasonStats,
                        new SeasonStats(
                            key,
                            singleSeasonStats.SeasonYear,
                            singleSeasonStats.LeagueId,
                            singleSeasonStats.LeagueStrength,
                            singleSeasonStats.MatchesPlayed,
                            singleSeasonStats.Wins,
                            singleSeasonStats.Losses,
                            singleSeasonStats.Draws,
                            singleSeasonStats.GoalsFor,
                            singleSeasonStats.GoalsAgainst)
                    );
                    contentResponse.TeamsStrengthDictionary[key] =
                        contentResponse.TeamsStrengthDictionary[key].WithSeasonStats(merged);
                }
            }
        }


        contentResponse.PriorLeagueStrength = totalMatches > 0 ? (float)totalGoals / totalMatches : 0f;

        contentResponse.TeamsStrengthDictionary = contentResponse.TeamsStrengthDictionary
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.WithLikelihood().WithPosterior(contentResponse.PriorLeagueStrength)
            );

        return contentResponse;
    }

    private SimulationContent CalculateSeasonStatsByMatchRounds(
        SimulationContent contentResponse,
        IEnumerable<MatchRound> matchRounds,
        LeagueRound leagueRound,
        float leagueStrength,
        ref int totalGoals,
        ref int totalMatches)
    {
        var seasonEnum = EnumMapper.StringtoSeasonEnum(leagueRound.SeasonYear);

        foreach (var matchRound in matchRounds)
        {
            UpdateTeamStats(contentResponse, matchRound.HomeTeamId, matchRound, seasonEnum, leagueRound.LeagueId, leagueStrength, true);
            UpdateTeamStats(contentResponse, matchRound.AwayTeamId, matchRound, seasonEnum, leagueRound.LeagueId, leagueStrength, false);

            totalGoals += matchRound.HomeGoals + matchRound.AwayGoals;
            totalMatches += 2;
        }

        return contentResponse;
    }

    private void UpdateTeamStats(
        SimulationContent response,
        Guid teamId,
        MatchRound matchRound,
        SeasonEnum seasonEnum,
        Guid leagueId,
        float leagueStrength,
        bool isHomeTeam)
    {
        if (response.TeamsStrengthDictionary.TryGetValue(teamId, out var existingTeamStrength))
        {
            var updatedStats = _seasonStatsService.CalculateSeasonStats(matchRound, existingTeamStrength.SeasonStats, seasonEnum, leagueId, leagueStrength, isHomeTeam);
            response.TeamsStrengthDictionary[teamId] = existingTeamStrength with { SeasonStats = updatedStats };
        }
        else
        {
            var newTeamStrength = TeamStrength.Create(teamId, seasonEnum, leagueId, leagueStrength);
            var updatedStats = _seasonStatsService.CalculateSeasonStats(matchRound, newTeamStrength.SeasonStats, seasonEnum, leagueId, leagueStrength, isHomeTeam);
            newTeamStrength = newTeamStrength with { SeasonStats = updatedStats };
            response.TeamsStrengthDictionary.Add(teamId, newTeamStrength);
        }
    }
}
