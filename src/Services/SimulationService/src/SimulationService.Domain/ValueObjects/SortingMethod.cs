using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public class SortingMethod
{
    public string SortingOption { get; set; }
    public string Direction { get; set; } = "DESC";
    public string Condition { get; set; }
}
