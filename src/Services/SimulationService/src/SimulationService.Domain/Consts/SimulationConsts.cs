using System;

namespace SimulationService.Domain.Consts;

public static class SimulationConsts
{
    /// <summary>
    /// Opóźnienie jednej iteracji (ms)
    /// </summary>
    public const int ITERATION_DELAY = 500;

    /// <summary>
    /// Poziom ufności do danych bayesowskich (priory)
    /// </summary>
    public const float SIMULATION_CONFIDENCE_LEVEL = 0.95f; // 95% confidence level

    /// <summary>
    /// Maksymalna liczba iteracji przy jednej symulacji
    /// </summary>
    public const int MAX_SIMULATION_ITERATIONS = 10000;

    /// <summary>
    /// Ile meczów potrzeba, żeby ufać statystyce drużyny bardziej niż priory
    /// </summary>
    public const int GAMES_TO_REACH_TRUST = 2;

    /// <summary>
    /// Przewaga własnego boiska jako mnożnik lambda (np. 1.05 = +5%)
    /// </summary>
    public const float HOME_ADVANTAGE = 1.05f;

    /// <summary>
    /// Poziom wariancji losowej, np. 0.1 = ±10% rozrzut względem wyliczonych lambda
    /// </summary>
    public const float NOISE_FACTOR = 0.10f; // lub 0.15f dla ±15%
}
