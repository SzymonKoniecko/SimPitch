using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services.SimulationModels;

public class StandardPoissonStrategy : BaseSimulationStrategy
{
    public override (int HomeGoals, int AwayGoals) SimulateMatch(
        TeamStrength homeTeam, TeamStrength awayTeam, float leagueAverageGoals, SimulationParams simParams, Random rng)
    {

        double lambdaHome = CalculateLambda(
            homeTeam.Posterior.Offensive,
            awayTeam.Posterior.Defensive,
            leagueAverageGoals,
            simParams.HomeAdvantage,
            simParams.NoiseFactor,
            rng);

        double lambdaAway = CalculateLambda(
            awayTeam.Posterior.Offensive,
            homeTeam.Posterior.Defensive,
            leagueAverageGoals,
            1.0f, 
            simParams.NoiseFactor,
            rng);

        return (SamplePoisson(lambdaHome, rng), SamplePoisson(lambdaAway, rng));
    }
}