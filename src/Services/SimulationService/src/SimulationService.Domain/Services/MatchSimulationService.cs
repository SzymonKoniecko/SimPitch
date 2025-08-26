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

        public SimulationContent SimulationWorkflow(SimulationContent simulationContent)
        {
            foreach (var match in simulationContent.MatchRoundsToSimulate)
            {
                var homeTeam = simulationContent.TeamsStrengthDictionary[match.HomeTeamId];
                var awayTeam = simulationContent.TeamsStrengthDictionary[match.AwayTeamId];

                var (homeGoals, awayGoals) = SimulateMatch(
                    homeTeam,
                    awayTeam,
                    homeAdvantage: 1.05
                );

                match.HomeGoals = homeGoals;
                match.AwayGoals = awayGoals;
                match.IsPlayed = true;

                var homeStatsUpdated = homeTeam.SeasonStats.Increment(match, isHomeTeam: true);
                var awayStatsUpdated = awayTeam.SeasonStats.Increment(match, isHomeTeam: false);

                homeTeam = homeTeam with { SeasonStats = homeStatsUpdated };
                awayTeam = awayTeam with { SeasonStats = awayStatsUpdated };

                homeTeam = homeTeam.WithLikelihood().WithPosterior(simulationContent.PriorLeagueStrength);
                awayTeam = awayTeam.WithLikelihood().WithPosterior(simulationContent.PriorLeagueStrength);

                simulationContent.TeamsStrengthDictionary[homeTeam.TeamId] = homeTeam;
                simulationContent.TeamsStrengthDictionary[awayTeam.TeamId] = awayTeam;
            }
            return simulationContent;
        }

        private (int HomeGoals, int AwayGoals) SimulateMatch(
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
