using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.Entities;

public class SeasonStats
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public SeasonEnum SeasonYear { get; set; }
    public Guid LeagueId { get; set; }
    public int MatchesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public override string ToString() => $"Team ID: {TeamId}, Wins: {Wins}, Losses: {Losses}, Draws: {Draws}, Goals For: {GoalsFor}, Goals Against: {GoalsAgainst}";

    public int GetGoalsDifference() => GoalsFor - GoalsAgainst;
}