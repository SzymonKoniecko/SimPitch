using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public record SeasonStats
{
    public Guid TeamId { get; init; }
    public SeasonEnum SeasonYear { get; init; }
    public Guid LeagueId { get; init; }
    public float LeagueStrength { get; set; }
    public int MatchesPlayed { get; init; }
    public int Wins { get; init; }
    public int Losses { get; init; }
    public int Draws { get; init; }
    public int GoalsFor { get; init; }
    public int GoalsAgainst { get; init; }

    // Konstruktor ułatwiający mapowanie z encji
    public SeasonStats(Guid teamId, SeasonEnum seasonYear, Guid leagueId, float leagueStrength, int matchesPlayed, int wins, int losses, int draws, int goalsFor, int goalsAgainst)
    {
        TeamId = teamId;
        SeasonYear = seasonYear;
        LeagueId = leagueId;
        LeagueStrength = leagueStrength;
        MatchesPlayed = matchesPlayed;
        Wins = wins;
        Losses = losses;
        Draws = draws;
        GoalsFor = goalsFor;
        GoalsAgainst = goalsAgainst;
    }

    // Factory dla nowych obiektów
    public static SeasonStats CreateNew(Guid teamId, SeasonEnum seasonYear, Guid leagueId, float leagueStrength)
        => new SeasonStats(teamId, seasonYear, leagueId, leagueStrength, 0, 0, 0, 0, 0, 0);

    public override string ToString() => $"Team ID: {TeamId}, Wins: {Wins}, Losses: {Losses}, Draws: {Draws}, Goals For: {GoalsFor}, Goals Against: {GoalsAgainst}";

    public int GetGoalsDifference() => GoalsFor - GoalsAgainst;

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

    /// <summary>
    /// Merge both season stats
    /// </summary>
    /// <returns>currentSeasonStats with scaled data by LeagueStrength</returns>
    /// <exception cref="Exception"></exception>
    public SeasonStats Merge(SeasonStats currentSeasonStats, SeasonStats newSeasonStats)
    {
        if (currentSeasonStats.TeamId != newSeasonStats.TeamId)
            throw new Exception($"CANNOT MERGE DIFFERENT SEASON STATS IN {nameof(SeasonStats)}");

        float strengthFactor = newSeasonStats.LeagueStrength / currentSeasonStats.LeagueStrength;

        int matchesPlayed = currentSeasonStats.MatchesPlayed + newSeasonStats.MatchesPlayed;
        int wins = currentSeasonStats.Wins + newSeasonStats.Wins; 
        int losses = currentSeasonStats.Losses + newSeasonStats.Losses;
        int draws = currentSeasonStats.Draws + newSeasonStats.Draws;

        int goalsFor = currentSeasonStats.GoalsFor + (int)Math.Round(newSeasonStats.GoalsFor * strengthFactor);
        int goalsAgainst = currentSeasonStats.GoalsAgainst + (int)Math.Round(newSeasonStats.GoalsAgainst * strengthFactor);

        return currentSeasonStats with
        {
            MatchesPlayed = matchesPlayed,
            Wins = wins,
            Losses = losses,
            Draws = draws,
            GoalsFor = goalsFor,
            GoalsAgainst = goalsAgainst,
            LeagueStrength = currentSeasonStats.LeagueStrength
        };
    }
}
