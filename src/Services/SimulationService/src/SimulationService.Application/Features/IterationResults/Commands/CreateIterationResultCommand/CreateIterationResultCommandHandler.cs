using System;
using MediatR;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;

public class CreateIterationResultCommandHandler : IRequestHandler<CreateIterationResultCommand, bool>
{
    private readonly IIterationResultWriteRepository _IterationResultWriteRepository;
    public CreateIterationResultCommandHandler(IIterationResultWriteRepository IterationResultWriteRepository)
    {
        _IterationResultWriteRepository = IterationResultWriteRepository;
    }

    public async Task<bool> Handle(CreateIterationResultCommand command, CancellationToken cancellationToken)
    {
        await _IterationResultWriteRepository.CreateIterationResultAsync(IterationResultMapper.ToDomain(command.IterationResult), cancellationToken);
        
        return await Task.FromResult(true);
    }
}
