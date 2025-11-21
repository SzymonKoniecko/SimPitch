using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services.SimulationModels;

public class AdvancedSimulationStrategy : DixonColesStrategy
{
    public override (int HomeGoals, int AwayGoals) SimulateMatch(
        TeamStrength homeTeam, TeamStrength awayTeam, 
        float leagueAverageGoals, SimulationParams simParams, Random rng)
    {
        // Time Decay: Nowsze dane są ważniejsze.
        // Tutaj symulujemy to przez modyfikację lambdy w oparciu o datę ostatniej aktualizacji.
        // W pełnym modelu time-decay odbywa się na etapie estymacji parametrów, 
        // ale tutaj robimy "boost" dla drużyn w gazie (wygrywających ostatnio).

        double momentumHome = CalculateMomentum(homeTeam);
        double momentumAway = CalculateMomentum(awayTeam);

        // Wywołujemy logikę Dixon-Coles ze zmodyfikowanymi parametrami
        // Zwiększamy/zmniejszamy homeAdvantage dynamicznie

        var modifiedParams = new SimulationParams
        {
            // Kopiuj wszystkie pola ręcznie lub użyj jakiejś metody klonującej
            Title = simParams.Title,
            SeasonYears = simParams.SeasonYears,
            LeagueId = simParams.LeagueId,
            Iterations = simParams.Iterations,
            LeagueRoundId = simParams.LeagueRoundId,
            CreateScoreboardOnCompleteIteration = simParams.CreateScoreboardOnCompleteIteration,
            Seed = simParams.Seed,
            GamesToReachTrust = simParams.GamesToReachTrust,
            ConfidenceLevel = simParams.ConfidenceLevel,

            // Tutaj zmiana:
            HomeAdvantage = simParams.HomeAdvantage * (float)momentumHome,
            NoiseFactor = simParams.NoiseFactor
        };

        // Możemy tutaj Bivariate dodac ale 
        // bazujemy na Dixon-Coles jako fundamencie.
        return base.SimulateMatch(homeTeam, awayTeam, leagueAverageGoals, modifiedParams, rng);
    }

    private double CalculateMomentum(TeamStrength team)
    {
        // Prosta heurystyka momentum: jeśli drużyna strzelała dużo w ostatnich meczach
        if (team.SeasonStats.MatchesPlayed < 3) return 1.0;

        // Tutaj można by analizować historię z SeasonStats dokładniej
        // Zakładamy uproszczenie: Lepsze Likelihood vs Prior oznacza dobrą formę
        double ratio = team.Likelihood.Offensive / (team.Posterior.Offensive + 0.001);

        // Tłumimy wpływ (dampening)
        return 1.0 + (ratio - 1.0) * 0.1;
    }
}