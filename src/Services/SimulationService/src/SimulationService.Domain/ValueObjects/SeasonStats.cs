using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public record SeasonStats
{
    public Guid TeamId { get; init; }
    public SeasonEnum SeasonYear { get; init; }
    public Guid LeagueId { get; init; }
    public int MatchesPlayed { get; init; }
    public int Wins { get; init; }
    public int Losses { get; init; }
    public int Draws { get; init; }
    public int GoalsFor { get; init; }
    public int GoalsAgainst { get; init; }

    // Konstruktor ułatwiający mapowanie z encji
    public SeasonStats(Guid teamId, SeasonEnum seasonYear, Guid leagueId, int matchesPlayed, int wins, int losses, int draws, int goalsFor, int goalsAgainst)
    {
        TeamId = teamId;
        SeasonYear = seasonYear;
        LeagueId = leagueId;
        MatchesPlayed = matchesPlayed;
        Wins = wins;
        Losses = losses;
        Draws = draws;
        GoalsFor = goalsFor;
        GoalsAgainst = goalsAgainst;
    }

    // Factory dla nowych obiektów
    public static SeasonStats CreateNew(Guid teamId, SeasonEnum seasonYear, Guid leagueId)
        => new SeasonStats(teamId, seasonYear, leagueId, 0, 0, 0, 0, 0, 0);

    public override string ToString() => $"Team ID: {TeamId}, Wins: {Wins}, Losses: {Losses}, Draws: {Draws}, Goals For: {GoalsFor}, Goals Against: {GoalsAgainst}";

    public int GetGoalsDifference() => GoalsFor - GoalsAgainst;

    // Aktualizacja po meczu (immutable)
    public SeasonStats Increment(MatchRound matchRound, bool isHomeTeam)
    {
        int matchesPlayed = MatchesPlayed + 1;
        int wins = Wins, losses = Losses, draws = Draws;
        int goalsFor = GoalsFor, goalsAgainst = GoalsAgainst;

        if (isHomeTeam)
        {
            if (matchRound.HomeGoals > matchRound.AwayGoals) wins++;
            else if (matchRound.HomeGoals < matchRound.AwayGoals) losses++;
            else draws++;

            goalsFor += matchRound.HomeGoals;
            goalsAgainst += matchRound.AwayGoals;
        }
        else
        {
            if (matchRound.HomeGoals > matchRound.AwayGoals) losses++;
            else if (matchRound.HomeGoals < matchRound.AwayGoals) wins++;
            else draws++;

            goalsFor += matchRound.AwayGoals;
            goalsAgainst += matchRound.HomeGoals;
        }

        return this with
        {
            MatchesPlayed = matchesPlayed,
            Wins = wins,
            Losses = losses,
            Draws = draws,
            GoalsFor = goalsFor,
            GoalsAgainst = goalsAgainst
        };
    }

    public SeasonStats Merge(SeasonStats currentSeasonStats, SeasonStats newSeasonStats)
    {
        throw new NotImplementedException();
    }
}
