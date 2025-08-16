using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Domain.Entities;

public class SeasonStats
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string SeasonYear { get; set; }
    public Guid LeagueId { get; set; }
    public int MatchesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }

    public override string ToString() => $"Team ID: {TeamId}, Wins: {Wins}, Losses: {Losses}, Draws: {Draws}, Goals For: {GoalsFor}, Goals Against: {GoalsAgainst}";
}
