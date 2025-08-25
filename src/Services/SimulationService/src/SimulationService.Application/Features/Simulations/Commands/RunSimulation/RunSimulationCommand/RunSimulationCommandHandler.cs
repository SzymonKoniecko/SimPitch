using MediatR;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;

public class RunSimulationCommandHandler : IRequestHandler<RunSimulationCommand, string>
{
    private readonly IMediator _mediator;
    private readonly MatchSimulatorService _matchSimulator;

    public RunSimulationCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
        _matchSimulator = new MatchSimulatorService();
    }

    public async Task<string> Handle(RunSimulationCommand command, CancellationToken cancellationToken)
    {
        var simulationContent = await _mediator.Send(
            new InitSimulationContentCommand(command.SimulationParamsDto),
            cancellationToken
        );

        foreach (var match in simulationContent.MatchRoundsToSimulate)
        {
            var homeTeam = simulationContent.TeamsStrengthDictionary[match.HomeTeamId];
            var awayTeam = simulationContent.TeamsStrengthDictionary[match.AwayTeamId];

            var (homeGoals, awayGoals) = _matchSimulator.SimulateMatch(
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

        return GenerateReport(simulationContent);
    }

    private string GenerateReport(SimulationContent simulationContent)
    {
        
        var reportLines = simulationContent.MatchRoundsToSimulate.Select(m =>
        {
            var homeTeam = simulationContent.TeamsStrengthDictionary[m.HomeTeamId];
            var awayTeam = simulationContent.TeamsStrengthDictionary[m.AwayTeamId];

            return $"Mecz {m.HomeTeamId} vs {m.AwayTeamId}: {m.HomeGoals}-{m.AwayGoals} | " +
                $"Home Posterior Off: {homeTeam.Posterior.Offensive:F2}, Def: {homeTeam.Posterior.Defensive:F2} | " +
                $"Away Posterior Off: {awayTeam.Posterior.Offensive:F2}, Def: {awayTeam.Posterior.Defensive:F2}";
        });

        string report = string.Join(Environment.NewLine, reportLines);


        return report;
    }
}