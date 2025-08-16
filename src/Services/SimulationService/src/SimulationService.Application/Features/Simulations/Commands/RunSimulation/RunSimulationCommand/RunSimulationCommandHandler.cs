using System;
using MediatR;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;

public class RunSimulationCommandHandler : IRequestHandler<RunSimulationCommand, string>
{
    private readonly IMediator _mediator;

    public RunSimulationCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> Handle(RunSimulationCommand command, CancellationToken cancellationToken)
    {
        List<MatchRound> matchRounds = await _mediator.Send(new InitSimulationContentCommand(command.SimulationParamsDto));

        
        return Guid.NewGuid().ToString();
    }
}
