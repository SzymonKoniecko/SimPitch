using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public class GetIterationResultsBySimulationIdQueryHandler : IRequestHandler<GetIterationResultsBySimulationIdQuery, (List<IterationResultDto>, PagedResponseDetails)>
{
    private readonly IRedisSimulationRegistry _registry;

    private readonly IIterationResultReadRepository _IterationResultReadRepository;

    public GetIterationResultsBySimulationIdQueryHandler(IIterationResultReadRepository IterationResultReadRepository, IRedisSimulationRegistry registry)
    {
        _IterationResultReadRepository = IterationResultReadRepository;
        _registry = registry;
    }

    public async Task<(List<IterationResultDto>, PagedResponseDetails)> Handle(GetIterationResultsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        var pagedRequest = new PagedRequest(
            query.PagedRequest.Offset,
            query.PagedRequest.PageSize,
            EnumMapper.SortingOptionToEnum(query.PagedRequest.SortingMethod.SortingOption),
            query.PagedRequest.SortingMethod.Condition,
            query.PagedRequest.SortingMethod.Order
        );

        var cachedResults = await _registry.GetPagedIterationResults(pagedRequest, query.SimulationId, cancellationToken);
        if (cachedResults == null || cachedResults.Count() == 0)
        {
            var IterationResults = await _IterationResultReadRepository.GetIterationResultsBySimulationIdAsync(
                query.SimulationId, pagedRequest, cancellationToken);
            if (IterationResults != null && IterationResults.Count() > 0)
                await _registry.SetPagedIterationResults(pagedRequest, IterationResults, cancellationToken);

            return (
                IterationResults.Select(sr => IterationResultMapper.ToDto(sr)).ToList(),
                new PagedResponseDetails()
                {
                    TotalCount = await _IterationResultReadRepository.GetIterationResultsCountBySimulationId_AndStateAsync(query.SimulationId, cancellationToken),
                    PageNumber = (query.PagedRequest.Offset / query.PagedRequest.PageSize) + 1,
                    PageSize = query.PagedRequest.PageSize,
                    SortingOption = query.PagedRequest.SortingMethod.SortingOption,
                    Order = query.PagedRequest.SortingMethod.Order
                }
            );
        }
        return 
        (
            cachedResults.Select(sr => IterationResultMapper.ToDto(sr)).ToList(),
            new PagedResponseDetails()
            {
                TotalCount = await _IterationResultReadRepository.GetIterationResultsCountBySimulationId_AndStateAsync(query.SimulationId, cancellationToken),
                PageNumber = (query.PagedRequest.Offset / query.PagedRequest.PageSize) + 1,
                PageSize = query.PagedRequest.PageSize,
                SortingOption = query.PagedRequest.SortingMethod.SortingOption,
                Order = query.PagedRequest.SortingMethod.Order
            }
        );
    }
}
