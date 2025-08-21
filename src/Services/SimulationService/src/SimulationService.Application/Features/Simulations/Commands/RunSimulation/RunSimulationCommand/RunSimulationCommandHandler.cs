using MediatR;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Domain.Services;

public class RunSimulationCommandHandler : IRequestHandler<RunSimulationCommand, string>
{
    private readonly IMediator _mediator;
    private readonly MatchSimulatorService _matchSimulator;

    public RunSimulationCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
        _matchSimulator = new MatchSimulatorService(); // Seed opcjonalny dla powtarzalności
    }

    public async Task<string> Handle(RunSimulationCommand command, CancellationToken cancellationToken)
    {
        var initResponse = await _mediator.Send(
            new InitSimulationContentCommand(command.SimulationParamsDto),
            cancellationToken
        );

        foreach (var match in initResponse.MatchRoundsToSimulate)
        {
            var homeTeam = initResponse.TeamsStrengthDictionary[match.HomeTeamId];
            var awayTeam = initResponse.TeamsStrengthDictionary[match.AwayTeamId];

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

            homeTeam = homeTeam.WithLikelihood().WithPosterior(initResponse.PriorLeagueStrength);
            awayTeam = awayTeam.WithLikelihood().WithPosterior(initResponse.PriorLeagueStrength);

            initResponse.TeamsStrengthDictionary[homeTeam.TeamId] = homeTeam;
            initResponse.TeamsStrengthDictionary[awayTeam.TeamId] = awayTeam;
        }

        var reportLines = initResponse.MatchRoundsToSimulate.Select(m =>
        {
            var homeTeam = initResponse.TeamsStrengthDictionary[m.HomeTeamId];
            var awayTeam = initResponse.TeamsStrengthDictionary[m.AwayTeamId];

            return $"Mecz {m.HomeTeamId} vs {m.AwayTeamId}: {m.HomeGoals}-{m.AwayGoals} | " +
                $"Home Posterior Off: {homeTeam.Posterior.Offensive:F2}, Def: {homeTeam.Posterior.Defensive:F2} | " +
                $"Away Posterior Off: {awayTeam.Posterior.Offensive:F2}, Def: {awayTeam.Posterior.Defensive:F2}";
        });

        string report = string.Join(Environment.NewLine, reportLines);


        return report;
    }
}
