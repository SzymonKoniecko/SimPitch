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

    public SeasonStats(Guid teamId, SeasonEnum seasonYear, Guid leagueId, float leagueStrength, 
        int matchesPlayed, int wins, int losses, int draws, int goalsFor, int goalsAgainst)
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

    /// <summary>
    /// Factory
    /// </summary>
    public static SeasonStats CreateNew(Guid teamId, SeasonEnum seasonYear, Guid leagueId, float leagueStrength)
        => new SeasonStats(teamId, seasonYear, leagueId, leagueStrength, 0, 0, 0, 0, 0, 0);

    public override string ToString() 
        => $"Team: {TeamId}, Season: {SeasonYear}, Wins: {Wins}, Draws: {Draws}, Losses: {Losses}, " +
           $"Goals: {GoalsFor}-{GoalsAgainst}, Matches: {MatchesPlayed}";

    public int GetGoalsDifference() => GoalsFor - GoalsAgainst;

    /// <summary>
    /// Aktualizuje statystykę drużyny po rozegraniu pojedynczego meczu.
    /// </summary>
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
    /// Łączy statystyki dwóch sezonów (akumulacja historii drużyny).
    /// Zawsze bierze LeagueStrength z newData (bieżącego sezonu do symulacji).
    /// </summary>
    /// <param name="accumulator">Aktualne dane (zazwyczaj z symulacji bieżącego sezonu)</param>
    /// <param name="newData">Nowe dane do dodania (zazwyczaj dane historyczne z bazy)</param>
    /// <returns>Połączone statystyki z aktualizowaną LeagueStrength</returns>
    /// <exception cref="InvalidOperationException">Gdy TeamId się nie zgadza</exception>
    public SeasonStats Merge(SeasonStats accumulator, SeasonStats newData)
    {
        if (accumulator.TeamId != newData.TeamId)
            throw new InvalidOperationException(
                $"Cannot merge SeasonStats for different teams: {accumulator.TeamId} != {newData.TeamId}");

        return accumulator with
        {
            MatchesPlayed = accumulator.MatchesPlayed + newData.MatchesPlayed,
            Wins = accumulator.Wins + newData.Wins,
            Losses = accumulator.Losses + newData.Losses,
            Draws = accumulator.Draws + newData.Draws,
            GoalsFor = accumulator.GoalsFor + newData.GoalsFor,
            GoalsAgainst = accumulator.GoalsAgainst + newData.GoalsAgainst,

            LeagueStrength = newData.LeagueStrength,
            SeasonYear = newData.SeasonYear,
            LeagueId = newData.LeagueId
        };
    }
}
