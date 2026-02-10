using System;
using MediatR;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;

public class CreateIterationResultCommandHandler : IRequestHandler<CreateIterationResultCommand, bool>
{
    private readonly IIterationResultWriteRepository _IterationResultWriteRepository;
    private readonly IIterationResultReadRepository iterationResultReadRepository;

    public CreateIterationResultCommandHandler(
        IIterationResultWriteRepository IterationResultWriteRepository,
        IIterationResultReadRepository iterationResultReadRepository)
    {
        _IterationResultWriteRepository = IterationResultWriteRepository;
        this.iterationResultReadRepository = iterationResultReadRepository;
    }

    public async Task<bool> Handle(CreateIterationResultCommand command, CancellationToken cancellationToken)
    {
        if (command.IterationResult.IterationIndex == 0) // comes from ML
        {
            command.IterationResult.IterationIndex = await iterationResultReadRepository.GetIterationResultsCountBySimulationIdAsync(command.IterationResult.SimulationId, cancellationToken) + 1;
        }

        await _IterationResultWriteRepository.CreateIterationResultAsync(IterationResultMapper.ToDomain(command.IterationResult), cancellationToken);

        return await Task.FromResult(true);
    }
}
