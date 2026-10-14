namespace StatisticsService.Application.DTOs.Clients;

/// <summary>
/// Same JSON shape as StrengthItemDto of SimulationService. A ValueTuple was used before -
/// Newtonsoft expects Item1/Item2 for it, so the received values were silently lost (0, 0).
/// </summary>
public class StrengthItemDto
{
    public float Offensive { get; init; }
    public float Defensive { get; init; }
}
