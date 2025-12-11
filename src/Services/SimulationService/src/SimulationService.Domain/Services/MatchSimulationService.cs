using System;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces;
using SimulationService.Domain.Services.SimulationModels;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services
{
    public class MatchSimulatorService
    {
        private readonly Random _rng;
        private readonly SimulationModelType _modelType;
        private readonly IMatchSimulationStrategy _strategy;

        public MatchSimulatorService(int? seed = null, SimulationModelType modelType = SimulationModelType.StandardPoisson)
        {
            _rng = seed.HasValue ? new Random(seed.Value) : new Random();
            _modelType = modelType;
            _strategy = StrategyFactory.Create(modelType);
        }

        public SimulationContent SimulationWorkflow(SimulationContent simulationContent)
        {
            foreach (var match in simulationContent.MatchRoundsToSimulate)
            {
                var homeTeam = simulationContent.TeamsStrengthDictionary[match.HomeTeamId]
                    .OrderByDescending(x => x.LastUpdate).First();
                var awayTeam = simulationContent.TeamsStrengthDictionary[match.AwayTeamId]
                    .OrderByDescending(x => x.LastUpdate).First();

                var season = homeTeam.SeasonStats.SeasonYear; 
            
                float currentLeagueStrength = simulationContent.LeagueStrengths
                    .FirstOrDefault(ls => ls.SeasonYear == season)?.Strength 
                    ?? simulationContent.PriorLeagueStrength; // Fallback na prior

                var (homeGoals, awayGoals) = _strategy.SimulateMatch(
                    homeTeam,
                    awayTeam,
                    currentLeagueStrength,
                    simulationContent.SimulationParams,
                    _rng);

                // Reszta logiki bez zmian
                match.HomeGoals = homeGoals;
                match.AwayGoals = awayGoals;
                if (match.HomeGoals == match.AwayGoals) match.IsDraw = true;
                else match.IsDraw = false;
                match.IsPlayed = true;

                var homeStatsUpdated = homeTeam.SeasonStats.Increment(match, isHomeTeam: true);
                var awayStatsUpdated = awayTeam.SeasonStats.Increment(match, isHomeTeam: false);

                homeTeam = homeTeam with { SeasonStats = homeStatsUpdated };
                awayTeam = awayTeam with { SeasonStats = awayStatsUpdated };

                homeTeam = homeTeam.WithLikelihood().WithPosterior(simulationContent.PriorLeagueStrength, simulationContent.SimulationParams)
                    .UpdateTime().SetRoundId(match.RoundId).EnsureThatUpdatedTeamStrengthNotHaveInitialStatsId();
                awayTeam = awayTeam.WithLikelihood().WithPosterior(simulationContent.PriorLeagueStrength, simulationContent.SimulationParams)
                    .UpdateTime().SetRoundId(match.RoundId).EnsureThatUpdatedTeamStrengthNotHaveInitialStatsId();


                simulationContent.TeamsStrengthDictionary[homeTeam.TeamId].Add(homeTeam);
                simulationContent.TeamsStrengthDictionary[awayTeam.TeamId].Add(awayTeam);
            }

            return simulationContent;
        }
    }

    public static class StrategyFactory
    {
        public static IMatchSimulationStrategy Create(SimulationModelType type)
        {
            return type switch
            {
                SimulationModelType.StandardPoisson => new StandardPoissonStrategy(),
                SimulationModelType.DixonColes => new DixonColesStrategy(),
                SimulationModelType.BivariatePoisson => new BivariatePoissonStrategy(),
                SimulationModelType.Advanced => new AdvancedSimulationStrategy(),
                _ => new StandardPoissonStrategy()
            };
        }
    }
}