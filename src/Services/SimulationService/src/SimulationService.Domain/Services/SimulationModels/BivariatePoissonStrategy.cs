using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services.SimulationModels;

public class BivariatePoissonStrategy : BaseSimulationStrategy
{
    private const float LAMBDA_COVARIANCE = 0.2f; // Wspólny czynnik kowariancji

    public override (int HomeGoals, int AwayGoals) SimulateMatch(
        TeamStrength homeTeam, TeamStrength awayTeam, float 
            leagueAverageGoals, SimulationParams simParams, Random rng)
    {

        double lambdaHome = CalculateLambda(
            homeTeam.Posterior.Offensive, awayTeam.Posterior.Defensive,
            leagueAverageGoals,
            simParams.HomeAdvantage, simParams.NoiseFactor, rng);

        double lambdaAway = CalculateLambda(
            awayTeam.Posterior.Offensive, homeTeam.Posterior.Defensive,
            leagueAverageGoals,
            1.0f, simParams.NoiseFactor, rng);

        // Metoda trywialnej redukcji (trivariate reduction)
        // X = X1 + X3, Y = X2 + X3
        double l1 = Math.Max(0, lambdaHome - LAMBDA_COVARIANCE);
        double l2 = Math.Max(0, lambdaAway - LAMBDA_COVARIANCE);
        double l3 = LAMBDA_COVARIANCE;

        int x1 = SamplePoisson(l1, rng);
        int x2 = SamplePoisson(l2, rng);
        int x3 = SamplePoisson(l3, rng);

        return (x1 + x3, x2 + x3);
    }
}