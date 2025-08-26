using MediatR;
using SimulationService.Application.DomainValidators;
using SimulationService.Application.Features.SimulationResults.Commands.CreateSimulationResultCommand;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;

public class RunSimulationCommandHandler : IRequestHandler<RunSimulationCommand, Guid>
{
    private readonly IMediator _mediator;
    private readonly MatchSimulatorService _matchSimulator;
    

    public RunSimulationCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
        _matchSimulator = new MatchSimulatorService();
    }

    public async Task<Guid> Handle(RunSimulationCommand command, CancellationToken cancellationToken)
    {
        var simulationContent = await _mediator.Send(
            new InitSimulationContentCommand(command.SimulationParamsDto),
            cancellationToken
        );

        var validator = new SimulationContentValidator();
        var validationResult = validator.Validate(simulationContent);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }
        Guid simulationId = Guid.NewGuid();
        int simulationIndex = 0;
        List<MatchRound> matchRoundsToSimulateBackup = simulationContent.MatchRoundsToSimulate;

        for (int i = 0; i < command.SimulationParamsDto.Iterations; i++)
        {
            DateTime startTime = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (i > 0)
                simulationContent.MatchRoundsToSimulate = matchRoundsToSimulateBackup;

            simulationContent = _matchSimulator.SimulationWorkflow(simulationContent);
            watch.Stop();
            simulationIndex++;
            
            await _mediator.Send(new CreateSimulationResultCommand(
                SimulationResultMapper.SimulationToDto(
                    simulationId,
                    simulationIndex,
                    startTime,
                    watch.Elapsed,
                    simulationContent.MatchRoundsToSimulate,
                    simulationContent.LeagueStrength,
                    simulationContent.PriorLeagueStrength,
                    GenerateReport(simulationContent)
                )), cancellationToken);
        }

        return simulationId;
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