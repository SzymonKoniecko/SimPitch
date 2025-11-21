using System;
using SimulationService.Domain.Interfaces;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services;

public abstract class BaseSimulationStrategy : IMatchSimulationStrategy
{
    public abstract (int HomeGoals, int AwayGoals) SimulateMatch(
        TeamStrength homeTeam, TeamStrength awayTeam, 
        float leagueAverageGoals,
        SimulationParams simParams, Random rng);

    protected double CalculateLambda(
        float offensive, float defensive, float leagueAverageGoals, float advantage, float noiseFactor, Random rng)
    {
        // Zabezpieczenie przed zerem (fallback na 2.5 gola w meczu, czyli 1.25 na drużynę)
        if (leagueAverageGoals <= 0.01f) leagueAverageGoals = 2.5f;

        // LeagueStrength to zazwyczaj "bramki na mecz" (suma obu drużyn).
        // My potrzebujemy średniej na JEDNĄ drużynę.
        float avgPerTeam = leagueAverageGoals / 2.0f;

        // Formuła z normalizacją: (Atak * Obrona) / ŚredniaLigowa
        // Dzięki temu jeśli Atak=1.5 i Obrona=1.5 przy Średniej=1.5:
        // (1.5 * 1.5) / 1.5 = 1.5. Wynik jest stabilny.
        double lambda = (offensive * defensive) / avgPerTeam * advantage; 

        // Aplikacja szumu
        lambda *= 1f - noiseFactor + 2f * noiseFactor * rng.NextDouble();

        return lambda;
    }

    protected int SamplePoisson(double lambda, Random rng)
    {
        if (lambda <= 0) return 0;
        int k = 0;
        double p = 1.0;
        double L = Math.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= rng.NextDouble();
        }
        return k - 1;
    }

    protected double PoissonPMF(int k, double lambda)
    {
        return Math.Exp(-lambda) * Math.Pow(lambda, k) / Factorial(k);
    }

    private double Factorial(int n)
    {
        if (n <= 1) return 1;
        double result = 1;
        for (int i = 2; i <= n; i++) result *= i;
        return result;
    }
}