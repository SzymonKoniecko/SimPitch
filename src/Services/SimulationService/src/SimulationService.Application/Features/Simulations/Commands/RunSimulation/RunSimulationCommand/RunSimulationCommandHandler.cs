using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimulationService.Application.DomainValidators;
using SimulationService.Application.Extensions;
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
using SimulationService.Domain.ValueObjects;

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
        var commandValidator = new RunSimulationCommandValidator();
        var validationResult = commandValidator.Validate(command);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var simulationContent = await _mediator.Send(
            new InitSimulationContentCommand(command.SimulationParamsDto),
            cancellationToken
        );
        command.Overview.PriorLeagueStrength = simulationContent.PriorLeagueStrength;
        command.Overview.LeagueStrengthsJSON = JsonConvert.SerializeObject(simulationContent.LeagueStrengths);


        var validator = new SimulationContentValidator();
        validationResult = validator.Validate(simulationContent);

        _matchSimulator = new MatchSimulatorService(simulationContent.SimulationParams.Seed, command.SimulationParamsDto.ModelType);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }
        await _simulationOverviewWriteRepository.CreateSimulationOverviewAsync(command.Overview, cancellationToken);
        int simulationIndex = 0;

        var (initialMatchRounds, initialTeamStrengths) =
            DeepCloneExtensions.CloneSimulationDataManual(
                simulationContent.MatchRoundsToSimulate,
                simulationContent.TeamsStrengthDictionary
        );

        for (int i = 1; i <= command.SimulationParamsDto.Iterations; i++)
        {
            var (freshMatchRounds, freshTeamStrengths) = DeepCloneExtensions.CloneSimulationDataManual(
                    initialMatchRounds, 
                    initialTeamStrengths
            );


            cancellationToken.ThrowIfCancellationRequested();

            if (await _simulationStateReadRepository.IsSimulationStateCancelled(command.simulationId, cancellationToken))
            {
                _logger.LogInformation($"////Cancelled simulation before iteration: {i} -- simulationId: {command.simulationId}////");
                break;
            }


            _logger.LogInformation($"//Started simulation, iteration: {i} -- simulationId: {command.simulationId}//");

            DateTime startTime = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // the be sure that new iteration is not inheriting the values from previous iterations.
            simulationContent.MatchRoundsToSimulate = freshMatchRounds;
            simulationContent.TeamsStrengthDictionary = freshTeamStrengths;

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