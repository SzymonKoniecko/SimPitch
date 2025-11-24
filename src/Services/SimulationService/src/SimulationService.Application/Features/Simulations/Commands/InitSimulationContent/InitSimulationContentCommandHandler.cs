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
        List<Guid> leagueRoundsToClearForCustomSimulation = LeagueRoundSimulationHelper.FindLeagueRoundsForCustomSimulation(leagueRounds, contentResponse.SimulationParams.LeagueRoundId);


        // KROK 2: Akumuluj statystyki z meczów do symulacji
        int totalGoals = 0;
        int totalMatches = 0;
        League currentLeague = null;

        foreach (var leagueRound in leagueRounds)
        {
            // Pobierz ligę tylko raz, gdy się zmieni
            if (currentLeague == null || currentLeague.Id != leagueRound.LeagueId)
            {
                currentLeague = await _mediator.Send(new GetLeagueByIdQuery(leagueRound.LeagueId), cancellationToken);
                contentResponse.LeagueStrengths = currentLeague.LeagueStrengths;
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

            // Dodaj tylko mecze, które nie zostały jeszcze rozegrane
            contentResponse.MatchRoundsToSimulate.AddRange(
                matchRounds.Where(m => !m.IsPlayed));

            // Oblicz statystyki drużyn na podstawie meczów
            contentResponse = CalculateSeasonStatsByMatchRounds(
                contentResponse,
                matchRounds,
                leagueRound,
                currentLeague.LeagueStrengths,
                ref totalGoals,
                ref totalMatches);
        }

        // KROK 3: Jeśli symulujemy wiele sezonów, dołącz historyczne dane drużyn
        if (query.SimulationParamsDto.SeasonYears.Count() > 1)
        {
            await EnrichTeamsWithHistoricalData(
                contentResponse,
                query.SimulationParamsDto.SeasonYears,
                cancellationToken);
        }

        // KROK 4: Oblicz średnią siłę ligi na podstawie meczów do symulacji
        // (UWAGA: totalMatches to liczba meczów, nie drużyn)
        contentResponse.PriorLeagueStrength = totalMatches > 0
            ? (float)totalGoals / totalMatches
            : 2.5f; // Fallback na typową średnią

        EnsureAllTeamsHaveBaseStrength(contentResponse);

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
    private void EnsureAllTeamsHaveBaseStrength(SimulationContent contentResponse)
    {
        var allTeamIds = contentResponse.MatchRoundsToSimulate
            .SelectMany(m => new[] { m.HomeTeamId, m.AwayTeamId })
            .Distinct()
            .ToList();

        if (contentResponse.TeamsStrengthDictionary == null)
        {
            contentResponse.TeamsStrengthDictionary = new Dictionary<Guid, List<TeamStrength>>();
        }

        var currentSeasonStr = contentResponse.SimulationParams.SeasonYears.LastOrDefault()
                               ?? contentResponse.SimulationParams.SeasonYears.First();
        var currentSeasonEnum = EnumMapper.StringtoSeasonEnum(currentSeasonStr);
        var currentLeagueId = contentResponse.SimulationParams.LeagueId;

        var leagueStrength = contentResponse.LeagueStrengths?
            .FirstOrDefault(x => x.SeasonYear == currentSeasonEnum)?.Strength
            ?? 2.5f;

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
        List<LeagueStrength> leagueStrengths,
        ref int totalGoals,
        ref int totalMatches)
    {
        var seasonEnum = EnumMapper.StringtoSeasonEnum(leagueRound.SeasonYear);
        var leagueStrength = leagueStrengths
            .FirstOrDefault(x => x.SeasonYear == seasonEnum)?.Strength
            ?? 2.5f; // Fallback

        if (leagueStrength == 2.5f)
        {
            _logger.LogWarning(
                "Missing LeagueStrength for season {SeasonYear}, LeagueRoundId: {RoundId}. Using default 2.5",
                leagueRound.SeasonYear,
                leagueRound.Id);
        }

        foreach (var matchRound in matchRounds)
        {
            if (!matchRound.IsPlayed) continue;

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
            totalGoals += matchRound.HomeGoals + matchRound.AwayGoals;
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
    private async Task EnrichTeamsWithHistoricalData(
        SimulationContent contentResponse,
        IEnumerable<string> seasonYears,
        CancellationToken cancellationToken)
    {
        var requestedSeasons = seasonYears.ToHashSet();

        foreach (var (teamId, teamStrengthList) in contentResponse.TeamsStrengthDictionary)
        {
            // Pobierz historyczne statystyki drużyny
            var historicalStats = await _mediator.Send(
                new GetSeasonsStatsByTeamIdGrpcQuery(teamId),
                cancellationToken);

            if (historicalStats == null)
                continue; // Brak historii dla (zadnej?) drużyny

            // Filtruj i sortuj chronologicznie
            var filteredSeasons = historicalStats
                .Where(s => requestedSeasons.Contains(
                    EnumMapper.SeasonEnumToString(s.SeasonYear)) && s.TeamId == teamId)
                .OrderBy(s => s.SeasonYear) // Chronologicznie: stare → nowe
                .ToList();

            if (!filteredSeasons.Any())
                continue; // Brak historii dla tej drużyny

            // Rozpocznij akumulację od obecnych statystyk drużyny
            var accumulated = teamStrengthList.First().SeasonStats;

            // Merge chronologicznie (stary → nowy)
            foreach (var historicalSeason in filteredSeasons)
            {
                var historicalStats_Dto = new SeasonStats(
                    teamId,
                    historicalSeason.SeasonYear,
                    historicalSeason.LeagueId,
                    historicalSeason.LeagueStrength,
                    historicalSeason.MatchesPlayed,
                    historicalSeason.Wins,
                    historicalSeason.Losses,
                    historicalSeason.Draws,
                    historicalSeason.GoalsFor,
                    historicalSeason.GoalsAgainst);

                accumulated = accumulated.Merge(accumulated, historicalStats_Dto);
            }

            // Zaktualizuj drużynę z połączonymi statystykami
            contentResponse.TeamsStrengthDictionary[teamId] = new List<TeamStrength>
            {
                teamStrengthList.First().WithSeasonStats(accumulated)
            };

            _logger.LogInformation(
                "Team {TeamId} enriched with historical data. Total matches: {TotalMatches}",
                teamId,
                accumulated.MatchesPlayed);
        }
    }
}
