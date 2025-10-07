using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public class GetIterationResultsBySimulationIdQueryHandler : IRequestHandler<GetIterationResultsBySimulationIdQuery, List<IterationResultDto>>
{
    private readonly IIterationResultReadRepository _IterationResultReadRepository;

    public GetIterationResultsBySimulationIdQueryHandler(IIterationResultReadRepository IterationResultReadRepository)
    {
        _IterationResultReadRepository = IterationResultReadRepository;
    }

    public async Task<List<IterationResultDto>> Handle(GetIterationResultsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        var IterationResults = await _IterationResultReadRepository.GetIterationResultsBySimulationIdAsync(query.SimulationId, cancellationToken);

        return IterationResults.Select(sr => IterationResultMapper.ToDto(sr)).ToList();
    }
}
