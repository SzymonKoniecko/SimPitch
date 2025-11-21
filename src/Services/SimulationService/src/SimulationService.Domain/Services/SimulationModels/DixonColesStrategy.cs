using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services.SimulationModels;

public class DixonColesStrategy : BaseSimulationStrategy
{
    private const float RHO = -0.05f; // Param for low scores in matches
    private const int MAX_GOALS = 10;

    public override (int HomeGoals, int AwayGoals) SimulateMatch(
        TeamStrength homeTeam, TeamStrength awayTeam,
            float leagueAverageGoals, SimulationParams simParams, Random rng)
    {

        double lambdaHome = CalculateLambda(
            homeTeam.Posterior.Offensive, awayTeam.Posterior.Defensive,
            leagueAverageGoals,
            simParams.HomeAdvantage, simParams.NoiseFactor, rng);

        double lambdaAway = CalculateLambda(
            awayTeam.Posterior.Offensive, homeTeam.Posterior.Defensive,
            leagueAverageGoals,
            1.0f, simParams.NoiseFactor, rng);

        // Budowa macierzy prawdopodobieństw z korektą tau
        double[,] probMatrix = new double[MAX_GOALS, MAX_GOALS];
        double sum = 0;

        for (int x = 0; x < MAX_GOALS; x++)
        {
            for (int y = 0; y < MAX_GOALS; y++)
            {
                double tau = TauCorrection(x, y, lambdaHome, lambdaAway, RHO);
                double prob = PoissonPMF(x, lambdaHome) * PoissonPMF(y, lambdaAway) * tau;
                probMatrix[x, y] = prob;
                sum += prob;
            }
        }

        // Normalizacja i losowanie
        double roll = rng.NextDouble() * sum;
        double cumulative = 0;

        for (int x = 0; x < MAX_GOALS; x++)
        {
            for (int y = 0; y < MAX_GOALS; y++)
            {
                cumulative += probMatrix[x, y];
                if (roll <= cumulative)
                    return (x, y);
            }
        }

        return (0, 0);
    }

    private double TauCorrection(int x, int y, double lambdaHome, double lambdaAway, float rho)
    {
        if (x == 0 && y == 0) return 1 - (lambdaHome * lambdaAway * rho);
        if (x == 0 && y == 1) return 1 + (lambdaHome * rho);
        if (x == 1 && y == 0) return 1 + (lambdaAway * rho);
        if (x == 1 && y == 1) return 1 - rho;
        return 1.0;
    }
}