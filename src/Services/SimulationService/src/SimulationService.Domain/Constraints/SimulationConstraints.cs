using System;

namespace SimulationService.Domain.Constraints;

public static class SimulationConstraints
{
    public const float SIMULATION_CONFIDENCE_LEVEL = 0.95f; // 95% confidence level for simulations
    public const int MAX_SIMULATION_ITERATIONS = 10000; // Maximum iterations for simulation runs
    public const int GAMES_TO_REACH_TRUST = 2; // Number of games to reach a trusted simulation state
}
