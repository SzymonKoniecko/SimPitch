using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public class GetIterationResultsBySimulationIdQueryHandler : IRequestHandler<GetIterationResultsBySimulationIdQuery, (List<IterationResultDto>, PagedResponseDetails)>
{
    private readonly IIterationResultReadRepository _IterationResultReadRepository;

    public GetIterationResultsBySimulationIdQueryHandler(IIterationResultReadRepository IterationResultReadRepository)
    {
        _IterationResultReadRepository = IterationResultReadRepository;
    }

    public async Task<(List<IterationResultDto>, PagedResponseDetails)> Handle(GetIterationResultsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        var IterationResults = await _IterationResultReadRepository.GetIterationResultsBySimulationIdAsync(
            query.SimulationId,
            new PagedRequest(
                query.PagedRequest.Offset,
                query.PagedRequest.PageSize,
                EnumMapper.SortingOptionToEnum(query.PagedRequest.SortingMethod.SortingOption),
                query.PagedRequest.SortingMethod.Order
            ), cancellationToken);

        return (
            IterationResults.Select(sr => IterationResultMapper.ToDto(sr)).ToList(),
            new PagedResponseDetails()
            {
                TotalCount = await _IterationResultReadRepository.GetIterationResultsCountBySimulationIdAsync(query.SimulationId, cancellationToken),
                PageNumber = (query.PagedRequest.Offset / query.PagedRequest.PageSize) + 1,
                PageSize = query.PagedRequest.PageSize,
                SortingOption = query.PagedRequest.SortingMethod.SortingOption,
                Order = query.PagedRequest.SortingMethod.Order
            }
        );
    }
}
