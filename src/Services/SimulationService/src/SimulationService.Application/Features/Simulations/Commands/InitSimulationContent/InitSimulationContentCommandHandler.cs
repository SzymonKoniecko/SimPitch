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
using Microsoft.Extensions.Logging;

public partial class InitSimulationContentCommandHandler : IRequestHandler<InitSimulationContentCommand, SimulationContent>
{
    private readonly SeasonStatsService _seasonStatsService;
    private readonly IMediator _mediator;
    private readonly ILogger<InitSimulationContentCommandHandler> _logger;

    public InitSimulationContentCommandHandler(SeasonStatsService seasonStatsService, IMediator mediator, ILogger<InitSimulationContentCommandHandler> logger)
    {
        _seasonStatsService = seasonStatsService ?? throw new ArgumentNullException(nameof(seasonStatsService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger;
    }

    public async Task<SimulationContent> Handle(InitSimulationContentCommand query, CancellationToken cancellationToken)
    {
        SimulationContent contentResponse = new();
        contentResponse.SimulationParams = SimulationParamsMapper.ToValueObject(query.SimulationParamsDto);

        var leagueRoundDtoRequest = new LeagueRoundDtoRequest
        {
            SeasonYears = contentResponse.SimulationParams.SeasonYears,
            LeagueId = contentResponse.SimulationParams.LeagueId,
            LeagueRoundId = contentResponse.SimulationParams.LeagueRoundId,
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
                contentResponse.LeagueStrengths = league.LeagueStrengths;
            }

            var matchRounds = await _mediator.Send(new GetMatchRoundsByIdQuery(leagueRound.Id));
            contentResponse.MatchRoundsToSimulate.AddRange(matchRounds.Where(m => !m.IsPlayed));

            contentResponse = CalculateSeasonStatsByMatchRounds(
                contentResponse,
                matchRounds,
                leagueRound,
                league.LeagueStrengths,
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
                    var merged = value.First().SeasonStats.Merge(
                        value.First().SeasonStats,
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
                        new List<TeamStrength> { contentResponse.TeamsStrengthDictionary[key].First().WithSeasonStats(merged) };
                }
            }
        }


        contentResponse.PriorLeagueStrength = totalMatches > 0 ? (float)totalGoals / totalMatches : 0f;

        contentResponse.TeamsStrengthDictionary = contentResponse.TeamsStrengthDictionary
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(x => x.WithLikelihood().WithPosterior(contentResponse.PriorLeagueStrength, contentResponse.SimulationParams)).ToList()
            );

        return contentResponse;
    }

    private SimulationContent CalculateSeasonStatsByMatchRounds(
        SimulationContent contentResponse,
        IEnumerable<MatchRound> matchRounds,
        LeagueRound leagueRound,
        List<LeagueStrength> leagueStrengths,
        ref int totalGoals,
        ref int totalMatches)
    {
        var seasonEnum = EnumMapper.StringtoSeasonEnum(leagueRound.SeasonYear);
        var leagueStrength = leagueStrengths.FirstOrDefault(x => x.SeasonYear == seasonEnum)?.Strength ?? 1.00f;
        if (leagueStrength == 1.00f)
            _logger.LogWarning($"***/ Missing league strength! LeagueRoundId:{leagueRound.Id} for seasonYear: {leagueRound.SeasonYear}/*** ___Used 1.00f___");

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
            var updatedStats = _seasonStatsService.CalculateSeasonStats(matchRound, existingTeamStrength.First().SeasonStats, seasonEnum, leagueId, leagueStrength, isHomeTeam);
            // create a mutable copy of the existing list and replace the first entry with the updated stats
            var updatedList = existingTeamStrength.ToList();
            updatedList[0] = updatedList[0] with { SeasonStats = updatedStats };
            response.TeamsStrengthDictionary[teamId] = updatedList;
        }
        else
        {
            var newTeamStrength = TeamStrength.Create(teamId, seasonEnum, leagueId, leagueStrength);
            var updatedStats = _seasonStatsService.CalculateSeasonStats(matchRound, newTeamStrength.SeasonStats, seasonEnum, leagueId, leagueStrength, isHomeTeam);
            newTeamStrength = newTeamStrength with { SeasonStats = updatedStats };
            // add a list containing the new team strength
            response.TeamsStrengthDictionary.Add(teamId, new List<TeamStrength> { newTeamStrength });
        }
    }
}
