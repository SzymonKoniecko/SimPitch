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
                var homeTeam = simulationContent.TeamsStrengthDictionary[match.HomeTeamId].OrderByDescending(x => x.LastUpdate).First();
                var awayTeam = simulationContent.TeamsStrengthDictionary[match.AwayTeamId].OrderByDescending(x => x.LastUpdate).First();

                var (homeGoals, awayGoals) = SimulateMatch(homeTeam, awayTeam, simulationContent.SimulationParams.HomeAdvantage, simulationContent.SimulationParams.NoiseFactor);

                match.HomeGoals = homeGoals;
                match.AwayGoals = awayGoals;

                if (match.HomeGoals == match.AwayGoals)
                    match.IsDraw = true;

                match.IsPlayed = true;

                var homeStatsUpdated = homeTeam.SeasonStats.Increment(match, isHomeTeam: true);
                var awayStatsUpdated = awayTeam.SeasonStats.Increment(match, isHomeTeam: false);

                homeTeam = homeTeam with { SeasonStats = homeStatsUpdated };
                awayTeam = awayTeam with { SeasonStats = awayStatsUpdated };

                homeTeam = homeTeam.WithLikelihood().WithPosterior(simulationContent.PriorLeagueStrength, simulationContent.SimulationParams).UpdateTime().SetRoundId(match.RoundId);
                awayTeam = awayTeam.WithLikelihood().WithPosterior(simulationContent.PriorLeagueStrength, simulationContent.SimulationParams).UpdateTime().SetRoundId(match.RoundId);

                simulationContent.TeamsStrengthDictionary[homeTeam.TeamId].Add(homeTeam);
                simulationContent.TeamsStrengthDictionary[awayTeam.TeamId].Add(awayTeam);
            }

            return simulationContent;
        }

        private (int HomeGoals, int AwayGoals) SimulateMatch(
            TeamStrength homeTeam,
            TeamStrength awayTeam,
            float homeAdvantage,
            float noiceFactor)
        {
            if (homeTeam == null) throw new ArgumentNullException(nameof(homeTeam));
            if (awayTeam == null) throw new ArgumentNullException(nameof(awayTeam));

            double homeFactor = Math.Sqrt(homeTeam.SeasonStats.MatchesPlayed + 1);
            double awayFactor = Math.Sqrt(awayTeam.SeasonStats.MatchesPlayed + 1);

            double lambdaHome = homeTeam.Posterior.Offensive / awayFactor * awayTeam.Posterior.Defensive * homeAdvantage;
            double lambdaAway = awayTeam.Posterior.Offensive / homeFactor * homeTeam.Posterior.Defensive;

            lambdaHome *= 1f - noiceFactor + 2f * noiceFactor * _rng.NextDouble();
            lambdaAway *= 1f - noiceFactor + 2f * noiceFactor * _rng.NextDouble();

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
