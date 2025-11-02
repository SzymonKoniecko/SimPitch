using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public class SortingMethod
{
    public SortingOptionEnum SortingOption { get; set; }
    public string Order { get; set; } = "DESC";
}
