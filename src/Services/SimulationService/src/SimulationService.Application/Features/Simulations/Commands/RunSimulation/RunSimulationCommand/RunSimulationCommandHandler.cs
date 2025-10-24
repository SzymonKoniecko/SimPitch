using MediatR;
using Newtonsoft.Json;
using SimulationService.Application.DomainValidators;
using SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;

public class RunSimulationCommandHandler : IRequestHandler<RunSimulationCommand, Guid>
{
    private readonly IMediator _mediator;
    private readonly ISimulationOverviewWriteRepository _simulationOverviewWriteRepository;
    private readonly MatchSimulatorService _matchSimulator;
    

    public RunSimulationCommandHandler(
        IMediator mediator,
        ISimulationOverviewWriteRepository simulationOverviewWriteRepository)
    {
        _mediator = mediator;
        _simulationOverviewWriteRepository = simulationOverviewWriteRepository;

        _matchSimulator = new MatchSimulatorService();
    }

    public async Task<Guid> Handle(RunSimulationCommand command, CancellationToken cancellationToken)
    {
        Guid simulationId = Guid.NewGuid();
        var simulationContent = await _mediator.Send(
            new InitSimulationContentCommand(command.SimulationParamsDto),
            cancellationToken
        );
        SimulationOverview simulationOverview = new();
        simulationOverview.Id = simulationId;
        simulationOverview.Title = $"Title: {command.SimulationParamsDto.SeasonYears.First()} -- {command.SimulationParamsDto.Iterations}";
        simulationOverview.CreatedDate = DateTime.Now;
        simulationOverview.SimulationParams = JsonConvert.SerializeObject(command.SimulationParamsDto);

        await _simulationOverviewWriteRepository.CreateSimulationOverviewAsync(simulationOverview, cancellationToken);

        var validator = new SimulationContentValidator();
        var validationResult = validator.Validate(simulationContent);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }
        int simulationIndex = 0;
        List<MatchRound> matchRoundsToSimulateBackup = simulationContent.MatchRoundsToSimulate;

        for (int i = 0; i < command.SimulationParamsDto.Iterations; i++)
        {
            DateTime startTime = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (i > 0) // the be sure that matches are not updated in his first iteration
                simulationContent.MatchRoundsToSimulate = matchRoundsToSimulateBackup;

            simulationContent = _matchSimulator.SimulationWorkflow(simulationContent);
            watch.Stop();


            simulationIndex++;
            

            await _mediator.Send(new CreateIterationResultCommand(
                IterationResultMapper.SimulationToDto(
                    simulationId,
                    simulationIndex,
                    startTime,
                    watch.Elapsed,
                    simulationContent.MatchRoundsToSimulate,
                    simulationContent.LeagueStrength,
                    simulationContent.PriorLeagueStrength,
                    simulationContent.TeamsStrengthDictionary
                )), cancellationToken);
        }

        return simulationId;
    }
}