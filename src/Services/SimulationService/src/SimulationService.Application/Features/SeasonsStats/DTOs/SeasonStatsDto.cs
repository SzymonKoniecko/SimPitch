using System;
using Newtonsoft.Json;
using SimulationService.Domain.Enums;

namespace SimulationService.Application.Features.SeasonsStats.DTOs;

public class SeasonStatsDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public SeasonEnum SeasonYear { get; set; }
    public Guid LeagueId { get; set; }
    public float LeagueStrength { get; set; }
    public int MatchesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
}