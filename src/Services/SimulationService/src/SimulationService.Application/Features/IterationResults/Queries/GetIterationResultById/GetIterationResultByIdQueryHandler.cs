using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultById;

public class GetIterationResultByIdQueryHandler : IRequestHandler<GetIterationResultByIdQuery, IterationResultDto>
{
    private readonly IIterationResultReadRepository _iterationResultReadRepository;

    public GetIterationResultByIdQueryHandler(IIterationResultReadRepository iterationResultReadRepository)
    {
        _iterationResultReadRepository = iterationResultReadRepository;
    }

    public async Task<IterationResultDto> Handle(GetIterationResultByIdQuery query, CancellationToken cancellationToken)
    {
        return IterationResultMapper.ToDto(await _iterationResultReadRepository.GetIterationResultByIdAsync(query.iterationId, cancellationToken));
    } 
}
