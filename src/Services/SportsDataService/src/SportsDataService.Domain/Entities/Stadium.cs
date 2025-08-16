using System;

namespace SportsDataService.Domain.Entities;

public class Stadium
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; } = 0;
}
