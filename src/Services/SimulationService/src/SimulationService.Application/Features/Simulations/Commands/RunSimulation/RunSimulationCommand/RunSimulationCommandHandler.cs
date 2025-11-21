using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimulationService.Application.DomainValidators;
using SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;
using SimulationService.Application.Features.Scoreboards.Commands;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Domain.Services;

public class RunSimulationCommandHandler : IRequestHandler<RunSimulationCommand, Guid>
{
    private readonly IMediator _mediator;
    private readonly IRedisSimulationRegistry _registry;
    private readonly ILogger<RunSimulationCommandHandler> _logger;
    private readonly ISimulationStateWriteRepository _simulationStateWriteRepository;
    private readonly ISimulationStateReadRepository _simulationStateReadRepository;
    private readonly ISimulationOverviewWriteRepository _simulationOverviewWriteRepository;
    private MatchSimulatorService _matchSimulator;
    

    public RunSimulationCommandHandler(
        IMediator mediator,
        IRedisSimulationRegistry registry,
        ILogger<RunSimulationCommandHandler> logger,
        ISimulationStateWriteRepository simulationStateWriteRepository,
        ISimulationStateReadRepository simulationStateReadRepository,
        ISimulationOverviewWriteRepository simulationOverviewWriteRepository)
    {
        _mediator = mediator;
        _registry = registry;
        _logger = logger;
        _simulationStateWriteRepository = simulationStateWriteRepository;
        _simulationStateReadRepository = simulationStateReadRepository;
        _simulationOverviewWriteRepository = simulationOverviewWriteRepository;
        _matchSimulator = new MatchSimulatorService();
    }

    public async Task<Guid> Handle(RunSimulationCommand command, CancellationToken cancellationToken)
    {
        var simulationContent = await _mediator.Send(
            new InitSimulationContentCommand(command.SimulationParamsDto),
            cancellationToken
        );
        command.Overview.PriorLeagueStrength = simulationContent.PriorLeagueStrength;
        command.Overview.LeagueStrengthsJSON = JsonConvert.SerializeObject(simulationContent.LeagueStrengths);

        await _simulationOverviewWriteRepository.CreateSimulationOverviewAsync(command.Overview, cancellationToken);

        var validator = new SimulationContentValidator();
        var validationResult = validator.Validate(simulationContent);

        _matchSimulator = new MatchSimulatorService(simulationContent.SimulationParams.Seed, command.SimulationParamsDto.ModelType);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }
        int simulationIndex = 0;
        List<MatchRound> matchRoundsToSimulateBackup = simulationContent.MatchRoundsToSimulate;

        for (int i = 1; i <= command.SimulationParamsDto.Iterations; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await _simulationStateReadRepository.IsSimulationStateCancelled(command.simulationId, cancellationToken))
            {
                _logger.LogInformation($"////Cancelled simulation before iteration: {i} -- simulationId: {command.simulationId}////");
                break;
            }


            _logger.LogInformation($"//Started simulation, iteration: {i} -- simulationId: {command.simulationId}//");

            DateTime startTime = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (i > 1) // the be sure that matches are not updated in his first iteration
                simulationContent.MatchRoundsToSimulate = matchRoundsToSimulateBackup;

            simulationContent = _matchSimulator.SimulationWorkflow(simulationContent);
            watch.Stop();


            simulationIndex++;

            var itResultDto = IterationResultMapper.SimulationToIterationResultDto
            (
                command.simulationId,
                simulationIndex,
                startTime,
                watch.Elapsed,
                simulationContent.MatchRoundsToSimulate,
                simulationContent.TeamsStrengthDictionary
            );

            await _mediator.Send(new CreateIterationResultCommand(itResultDto), cancellationToken);

            command.State.LastCompletedIteration = i;
            await _registry.SetStateAsync(command.simulationId, command.State.Update((float)i / command.SimulationParamsDto.Iterations * 100));
            await _simulationStateWriteRepository.UpdateOrCreateAsync(command.State, cancellationToken: cancellationToken);
            
            if (command.SimulationParamsDto.CreateScoreboardOnCompleteIteration)
            {
                if (await _mediator.Send(new CreateScoreboardByIterationResultCommand(SimulationOverviewMapper.ToDto(command.Overview), itResultDto), cancellationToken) == false)
                _logger.LogError($"Scoreboard is not created-> IterationResultId:{itResultDto.Id}, SimulationId: {itResultDto.SimulationId}");
            }
        }

        return command.simulationId;
    }
}