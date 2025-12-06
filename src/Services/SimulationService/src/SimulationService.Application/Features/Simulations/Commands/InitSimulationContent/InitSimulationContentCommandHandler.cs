using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.Leagues.Query.GetLeagueById;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
using SimulationService.Application.Features.SeasonsStats.Queries.GetSeasonsStatsByTeamIdGrpc;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Helpers;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Consts;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;

public partial class InitSimulationContentCommandHandler : IRequestHandler<InitSimulationContentCommand, SimulationContent>
{
    private readonly SeasonStatsService _seasonStatsService;
    private readonly IMediator _mediator;
    private readonly ILogger<InitSimulationContentCommandHandler> _logger;

    public InitSimulationContentCommandHandler(
        SeasonStatsService seasonStatsService,
        IMediator mediator,
        ILogger<InitSimulationContentCommandHandler> logger)
    {
        _seasonStatsService = seasonStatsService ?? throw new ArgumentNullException(nameof(seasonStatsService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger;
    }

    public async Task<SimulationContent> Handle(InitSimulationContentCommand query, CancellationToken cancellationToken)
    {
        SimulationContent contentResponse = new SimulationContent();
        contentResponse.SimulationParams = SimulationParamsMapper.ToValueObject(query.SimulationParamsDto);

        // KROK 1: Pobierz rundy ligowe i mecze do symulacji
        var leagueRoundDtoRequest = new LeagueRoundDtoRequest
        {
            SeasonYears = contentResponse.SimulationParams.SeasonYears,
            LeagueId = contentResponse.SimulationParams.LeagueId,
            //LeagueRoundId = contentResponse.SimulationParams.LeagueRoundId, no needs to use - we gonna filter out from all league rounds
        };

        var leagueRounds = await _mediator.Send(new GetLeagueRoundsByParamsGrpcQuery(leagueRoundDtoRequest), cancellationToken);
        leagueRounds = LeagueRoundSimulationHelper.FilterLeagueRoundsForCustomSimulationToFindLastLeagueRoundToPlay(leagueRounds, contentResponse.SimulationParams.TargetLeagueRoundId);
        List<Guid> leagueRoundsToClearForCustomSimulation = LeagueRoundSimulationHelper.FindLeagueRoundsForCustomSimulation(leagueRounds, contentResponse.SimulationParams.LeagueRoundId);


        // KROK 2: Akumuluj statystyki z meczów do symulacji
        int totalGoals = 0;
        int totalMatches = 0;
        League currentLeague = null;

        foreach (var leagueRound in leagueRounds) // zmienic na jedynie current_season - bo reszta ma SeasonStats (o ile nie bedzie nigdy symulacji wielu sezonow)
        {
            // Pobierz ligę tylko raz, gdy się zmieni
            if (currentLeague == null || currentLeague.Id != leagueRound.LeagueId)
            {
                currentLeague = await _mediator.Send(new GetLeagueByIdQuery(leagueRound.LeagueId), cancellationToken);
                if (!contentResponse.LeagueStrengths.Any(x => x.LeagueId == currentLeague.Id))
                {
                    contentResponse.LeagueStrengths.AddRange(
                        currentLeague.LeagueStrengths.Where(x => x.LeagueId == currentLeague.Id && query.SimulationParamsDto.SeasonYears.Any(y => y.Equals(EnumMapper.SeasonEnumToString(x.SeasonYear))))
                    );
                }
            }

            var matchRounds = await _mediator.Send(
                new GetMatchRoundsByIdQuery(leagueRound.Id),
                cancellationToken);

            if (query.SimulationParamsDto.LeagueRoundId != Guid.Empty &&
                leagueRoundsToClearForCustomSimulation.Contains(leagueRound.Id))
            {
                matchRounds = LeagueRoundSimulationHelper.SetCustomStartToSimulate(
                    matchRounds,
                    leagueRoundsToClearForCustomSimulation);
            }
            // KROK 1.1: dodaj zangazowane druzuny do slownika
            EnsureAllTeamsHaveBaseStrength(contentResponse, matchRounds);

            // Dodaj tylko mecze, które nie zostały jeszcze rozegrane
            contentResponse.MatchRoundsToSimulate.AddRange(
                matchRounds.Where(m => !m.IsPlayed));

            // Oblicz statystyki drużyn na podstawie meczów
            contentResponse = CalculateSeasonStatsByMatchRounds(
                contentResponse,
                matchRounds,
                leagueRound,
                ref totalGoals,
                ref totalMatches
            );
        }

        // KROK 3: Jeśli symulujemy wiele sezonów, dołącz historyczne dane drużyn
        if (query.SimulationParamsDto.SeasonYears.Count() > 1)
        {
            (contentResponse, totalGoals) = await EnrichTeamsWithHistoricalData(
                contentResponse,
                query.SimulationParamsDto.SeasonYears,
                totalGoals,
                cancellationToken
            );

            var league = await _mediator.Send(new GetLeagueByIdQuery(contentResponse.SimulationParams.LeagueId), cancellationToken);
            totalMatches += league.MaxRound * 9;
        }

        // KROK 4: Oblicz średnią siłę ligi na podstawie meczów do symulacji
        // (UWAGA: totalMatches to liczba meczów, nie drużyn)
        contentResponse.PriorLeagueStrength = totalMatches > 0
            ? (float)totalGoals / totalMatches
            : 2.5f; // Fallback na typową średnią

        // KROK 5: Oblicz Likelihood i Posterior dla każdej drużyny
        contentResponse.TeamsStrengthDictionary = contentResponse.TeamsStrengthDictionary
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(teamStrength =>
                {
                    var updatedTeam = teamStrength;

                    if (teamStrength.SeasonStats.MatchesPlayed > 0)
                    {
                        updatedTeam = updatedTeam.WithLikelihood();
                    }

                    // Zawsze licz Posterior (dla 0 meczów będzie to po prostu prior)
                    updatedTeam = updatedTeam.WithPosterior(
                        contentResponse.PriorLeagueStrength,
                        contentResponse.SimulationParams);

                    return updatedTeam;
                }).ToList()
            );


        // _logger.LogInformation(
        //     "Simulation content initialized. Teams: {TeamCount}, Matches to simulate: {MatchCount}, " +
        //     "Prior League Strength: {PriorStrength:F2}",
        //     contentResponse.TeamsStrengthDictionary.Count,
        //     contentResponse.MatchRoundsToSimulate.Count,
        //     contentResponse.PriorLeagueStrength);

        return contentResponse;
    }
    /// <summary>
    /// Ensures that every team appearing in MatchRoundsToSimulate has a base TeamStrength entry,
    /// even if the season starts from the very first round (no matches played yet).
    /// </summary>
    private void EnsureAllTeamsHaveBaseStrength(SimulationContent contentResponse, List<MatchRound> matchRounds)
    {
        var allTeamIds = matchRounds
            .SelectMany(m => new[] { m.HomeTeamId, m.AwayTeamId })
            .Distinct()
            .ToList();

        if (contentResponse.TeamsStrengthDictionary == null)
        {
            contentResponse.TeamsStrengthDictionary = new Dictionary<Guid, List<TeamStrength>>();
        }

        var currentSeasonEnum = SimulationConsts.CURRENT_SEASON;
        var currentLeagueId = contentResponse.SimulationParams.LeagueId;

        float leagueStrength = 2.5f;
        if (contentResponse.LeagueStrengths != null && contentResponse.LeagueStrengths.Count() > 0)
        {
            leagueStrength = contentResponse.LeagueStrengths?
            .FirstOrDefault(x => x.SeasonYear == currentSeasonEnum)?.Strength
            ?? 2.5f;
        }
        

        foreach (var teamId in allTeamIds)
        {
            if (!contentResponse.TeamsStrengthDictionary.ContainsKey(teamId))
            {
                var baseStrength = TeamStrength.Create(
                    teamId,
                    currentSeasonEnum,
                    currentLeagueId,
                    leagueStrength
                );

                contentResponse.TeamsStrengthDictionary.Add(
                    teamId,
                    new List<TeamStrength> { baseStrength }
                );
            }
        }
    }


    /// <summary>
    /// Oblicza statystyki drużyn na podstawie meczów z konkretnej rundy ligowej.
    /// </summary>
    private SimulationContent CalculateSeasonStatsByMatchRounds(
        SimulationContent contentResponse,
        IEnumerable<MatchRound> matchRounds,
        LeagueRound leagueRound,
        ref int totalGoals,
        ref int totalMatches)
    {
        var seasonEnum = EnumMapper.StringtoSeasonEnum(leagueRound.SeasonYear);
        var leagueStrength = contentResponse.LeagueStrengths
            .FirstOrDefault(x => x.SeasonYear == seasonEnum)?.Strength // dac mapper do jednego sezonu nizej albo i nie, bo to przyszle mecze
            ?? 0.0f; // Fallback

        if (leagueStrength == 0.0f)
        {
            _logger.LogWarning(
                "Missing LeagueStrength for season {SeasonYear}, LeagueRoundId: {RoundId}. Using default 2.5",
                leagueRound.SeasonYear,
                leagueRound.Id);
            leagueStrength = 2.5f;
            contentResponse.LeagueStrengths.Add(
                new LeagueStrength() { 
                    Id = Guid.Empty,
                    LeagueId = leagueRound.LeagueId,
                    SeasonYear = seasonEnum,
                    Strength = leagueStrength
            });
        }

        foreach (var matchRound in matchRounds)
        {
            if (!matchRound.IsPlayed) continue;

            if (matchRound.HomeGoals == null || matchRound.AwayGoals == null)
            {
                throw new ArgumentNullException($"Home goals or away goals are null !! MatchRoundId:{matchRound.Id} " + nameof(CalculateSeasonStatsByMatchRounds));
            }

            UpdateTeamStats(
                contentResponse,
                matchRound.HomeTeamId,
                matchRound,
                seasonEnum,
                leagueRound.LeagueId,
                leagueStrength,
                isHomeTeam: true);

            UpdateTeamStats(
                contentResponse,
                matchRound.AwayTeamId,
                matchRound,
                seasonEnum,
                leagueRound.LeagueId,
                leagueStrength,
                isHomeTeam: false);

            // Akumuluj bramki i mecze (dla PriorLeagueStrength)
            totalGoals += matchRound.HomeGoals.Value + matchRound.AwayGoals.Value;
            totalMatches += 1;
        }

        return contentResponse;
    }

    /// <summary>
    /// Aktualizuje statystyki pojedynczej drużyny po meczu.
    /// </summary>
    private void UpdateTeamStats(
        SimulationContent response,
        Guid teamId,
        MatchRound matchRound,
        SeasonEnum seasonEnum,
        Guid leagueId,
        float leagueStrength,
        bool isHomeTeam)
    {
        if (response.TeamsStrengthDictionary.TryGetValue(teamId, out var existingTeamStrengths))
        {
            var updatedStats = _seasonStatsService.CalculateSeasonStats(
                matchRound,
                existingTeamStrengths.First().SeasonStats,
                seasonEnum,
                leagueId,
                leagueStrength,
                isHomeTeam);

            var updatedList = existingTeamStrengths.ToList();
            updatedList[0] = updatedList[0] with { SeasonStats = updatedStats };
            response.TeamsStrengthDictionary[teamId] = updatedList;
        }
        else
        {
            var newTeamStrength = TeamStrength.Create(teamId, seasonEnum, leagueId, leagueStrength);
            var updatedStats = _seasonStatsService.CalculateSeasonStats(
                matchRound,
                newTeamStrength.SeasonStats,
                seasonEnum,
                leagueId,
                leagueStrength,
                isHomeTeam);

            response.TeamsStrengthDictionary.Add(
                teamId,
                new List<TeamStrength> { newTeamStrength with { SeasonStats = updatedStats } });
        }
    }

    /// <summary>
    /// Dołącza historyczne dane drużyn z poprzednich sezonów (jeśli symulujemy wiele sezonów).
    /// </summary>
    private async Task<(SimulationContent, int)> EnrichTeamsWithHistoricalData(
    SimulationContent contentResponse,
    IEnumerable<string> seasonYears,
    int totalGoals,
    CancellationToken cancellationToken)
    {
        var requestedSeasons = seasonYears.ToHashSet();

        foreach (var (teamId, teamStrengthList) in contentResponse.TeamsStrengthDictionary)
        {
            var historicalStats = await _mediator.Send(
                new GetSeasonsStatsByTeamIdGrpcQuery(teamId),
                cancellationToken);

            if (historicalStats == null)
                continue;

            var filteredSeasons = historicalStats
                .Where(s => requestedSeasons.Contains(EnumMapper.SeasonEnumToString(s.SeasonYear)) && s.TeamId == teamId)
                .OrderBy(s => s.SeasonYear)
                .ToList();

            if (!filteredSeasons.Any())
            {
                _logger.LogWarning($"No historical data for team: {teamId}");
                continue;
            }

            // Startujemy od istniejącego TeamStrength i jego SeasonStats
            var teamStrength = teamStrengthList.First();

            // Iteracyjna aktualizacja Posteriora po kolejnych sezonach
            foreach (var histSeason in filteredSeasons)
            {
                var histStatsDto = new SeasonStats(
                    teamId,
                    histSeason.SeasonYear,
                    histSeason.LeagueId,
                    histSeason.LeagueStrength,
                    histSeason.MatchesPlayed,
                    histSeason.Wins,
                    histSeason.Losses,
                    histSeason.Draws,
                    histSeason.GoalsFor,
                    histSeason.GoalsAgainst);

                totalGoals += histStatsDto.GoalsFor;

                // Łączymy statystyki do seasonStats (merge nie powinien nadpisywać, tylko akumulować)
                var mergedStats = teamStrength.SeasonStats.Merge(teamStrength.SeasonStats, histStatsDto);

                // Tworzymy refreshed TeamStrength z aktualnym seasonStats
                teamStrength = teamStrength
                    .WithSeasonStats(mergedStats)
                    .WithLikelihood() // Likelihood na podstawie sezonowych danych
                    .WithPosterior(histSeason.LeagueStrength, contentResponse.SimulationParams) // Posterior uwzględnia siłę ligi z danego sezonu
                    .WithExpectedGoalsFromPosterior();
            }

            // Aktualizujemy słownik
            contentResponse.TeamsStrengthDictionary[teamId] = new List<TeamStrength> { teamStrength };
        }

        return (contentResponse, totalGoals);
    }

}
 