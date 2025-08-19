using System;

namespace SimulationService.Domain.Entities;

public class League
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; } = Guid.Empty;
    public int MaxRound { get; set; }
    public float Strength { get; set; }
}
