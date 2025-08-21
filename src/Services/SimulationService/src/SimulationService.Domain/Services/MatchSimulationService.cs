using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services
{
    public class MatchSimulatorService
    {
        private readonly Random _rng;

        public MatchSimulatorService(int? seed = null)
        {
            _rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public (int HomeGoals, int AwayGoals) SimulateMatch(
            TeamStrength homeTeam,
            TeamStrength awayTeam,
            double homeAdvantage = 1.05)
        {
            if (homeTeam == null) throw new ArgumentNullException(nameof(homeTeam));
            if (awayTeam == null) throw new ArgumentNullException(nameof(awayTeam));

            double homeFactor = Math.Sqrt(homeTeam.SeasonStats.MatchesPlayed + 1);
            double awayFactor = Math.Sqrt(awayTeam.SeasonStats.MatchesPlayed + 1);

            double lambdaHome = homeTeam.Posterior.Offensive / awayFactor * awayTeam.Posterior.Defensive * homeAdvantage;
            double lambdaAway = awayTeam.Posterior.Offensive / homeFactor * homeTeam.Posterior.Defensive;

            lambdaHome *= 0.9 + 0.2 * _rng.NextDouble(); // between 0.9 a 1.1
            lambdaAway *= 0.9 + 0.2 * _rng.NextDouble();

            int goalsHome = SamplePoisson(lambdaHome);
            int goalsAway = SamplePoisson(lambdaAway);

            return (goalsHome, goalsAway);
        }

        private int SamplePoisson(double lambda)
        {
            if (lambda <= 0) return 0;

            int k = 0;
            double p = 1.0;
            double L = Math.Exp(-lambda);

            while (p > L)
            {
                k++;
                p *= _rng.NextDouble();
            }

            return k - 1;
        }
    }
}
