using System;

namespace SportsDataService.Domain.Entities;

public class Country
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public override string ToString() => $"{Name} ({Code})";
}
