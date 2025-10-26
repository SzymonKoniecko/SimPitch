using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using SimulationService.Application.DomainValidators;
using SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Domain.Services;

public class RunSimulationCommandHandler : IRequestHandler<RunSimulationCommand, Guid>
{
    private readonly IMediator _mediator;
    private readonly IRedisSimulationRegistry _registry;
    private readonly ILogger<RunSimulationCommandHandler> _logger;
    private readonly ISimulationStateWriteRepository _simulationStateWriteRepository;
    private readonly MatchSimulatorService _matchSimulator;
    

    public RunSimulationCommandHandler(
        IMediator mediator,
        IRedisSimulationRegistry registry,
        ILogger<RunSimulationCommandHandler> logger,
        ISimulationStateWriteRepository simulationStateWriteRepository)
    {
        _mediator = mediator;
        _registry = registry;
        _logger = logger;
        _simulationStateWriteRepository = simulationStateWriteRepository;
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
        int simulationIndex = 0;
        List<MatchRound> matchRoundsToSimulateBackup = simulationContent.MatchRoundsToSimulate;

        for (int i = 1; i <= command.SimulationParamsDto.Iterations; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            _logger.LogInformation($"Started simulation, iteration: {i} -- simulationId: {command.simulationId}");

            DateTime startTime = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (i > 1) // the be sure that matches are not updated in his first iteration
                simulationContent.MatchRoundsToSimulate = matchRoundsToSimulateBackup;

            simulationContent = _matchSimulator.SimulationWorkflow(simulationContent);
            watch.Stop();


            simulationIndex++;


            await _mediator.Send(new CreateIterationResultCommand(
                IterationResultMapper.SimulationToDto(
                    command.simulationId,
                    simulationIndex,
                    startTime,
                    watch.Elapsed,
                    simulationContent.MatchRoundsToSimulate,
                    simulationContent.LeagueStrength,
                    simulationContent.PriorLeagueStrength,
                    simulationContent.TeamsStrengthDictionary
                )), cancellationToken);

            command.State.LastCompletedIteration = i;
            await _registry.SetStateAsync(command.simulationId, command.State.Update((float)i / command.SimulationParamsDto.Iterations * 100));
            await _simulationStateWriteRepository.UpdateOrCreateAsync(command.State, cancellationToken: cancellationToken);
        }

        return command.simulationId;
    }
}