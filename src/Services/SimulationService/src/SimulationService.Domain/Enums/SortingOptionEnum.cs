namespace SimulationService.Domain.Enums;

public enum SortingOptionEnum
{
    /// <summary>
    /// Used in:
    /// -IterationPreview
    /// -SimulationOverview 
    /// </summary>
    CreatedDate,
    /// <summary>
    /// Used in:
    /// -IterationPreview
    /// </summary>
    ExecutionTime,
    /// <summary>
    /// Used in:
    /// -SimulationOverview   (+condition)
    /// </summary>
    Title,
    /// <summary>
    /// Used in:
    /// -IterationPreview
    /// -SimulationOverview 
    /// </summary>
    IterationResultNumber,
    /// <summary>
    /// Used in:
    /// -IterationPreview (not DB order)
    /// </summary>
    LeaderPoints,
    /// <summary>
    /// -SimulationOverview  (+condition)
    /// </summary>
    State
}