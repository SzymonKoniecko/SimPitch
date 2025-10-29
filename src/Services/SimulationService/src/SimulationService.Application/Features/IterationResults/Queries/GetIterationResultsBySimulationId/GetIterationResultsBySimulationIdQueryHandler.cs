using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public class GetIterationResultsBySimulationIdQueryHandler : IRequestHandler<GetIterationResultsBySimulationIdQuery, (List<IterationResultDto>, int)>
{
    private readonly IIterationResultReadRepository _IterationResultReadRepository;

    public GetIterationResultsBySimulationIdQueryHandler(IIterationResultReadRepository IterationResultReadRepository)
    {
        _IterationResultReadRepository = IterationResultReadRepository;
    }

    public async Task<(List<IterationResultDto>, int)> Handle(GetIterationResultsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        var IterationResults = await _IterationResultReadRepository.GetIterationResultsBySimulationIdAsync(query.SimulationId, query.offset, query.limit, cancellationToken);

        return (
            IterationResults.Select(sr => IterationResultMapper.ToDto(sr)).ToList(),
            await _IterationResultReadRepository.GetIterationResultsCountBySimulationIdAsync(query.SimulationId, cancellationToken));
    }
}
