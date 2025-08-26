using System;
using MediatR;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Application.Features.SimulationResults.Commands.CreateSimulationResultCommand;

public class CreateSimulationResultCommandHandler : IRequestHandler<CreateSimulationResultCommand, bool>
{
    private readonly ISimulationResultWriteRepository _simulationResultWriteRepository;
    public CreateSimulationResultCommandHandler(ISimulationResultWriteRepository simulationResultWriteRepository)
    {
        _simulationResultWriteRepository = simulationResultWriteRepository;
    }

    public async Task<bool> Handle(CreateSimulationResultCommand command, CancellationToken cancellationToken)
    {
        await _simulationResultWriteRepository.CreateSimulationResultAsync(SimulationResultMapper.ToDomain(command.SimulationResult), cancellationToken);
        
        return await Task.FromResult(true);
    }
}
