using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Enums;
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

        // Results of a running simulation keep changing - cache pages only once the simulation is finished,
        // otherwise a partially filled page would be served from the cache after completion.
        var isFinished = await IsSimulationFinishedAsync(query.SimulationId, cancellationToken);

        var cachedResults = isFinished
            ? await _registry.GetPagedIterationResults(pagedRequest, query.SimulationId, cancellationToken)
            : null;
        if (cachedResults == null || cachedResults.Count() == 0)
        {
            var IterationResults = await _IterationResultReadRepository.GetIterationResultsBySimulationIdAsync(
                query.SimulationId, pagedRequest, cancellationToken);
            if (isFinished && IterationResults != null && IterationResults.Count() > 0)
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

    private async Task<bool> IsSimulationFinishedAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var state = await _registry.GetStateAsync(simulationId, cancellationToken);
        return state?.State is SimulationStatus.Completed or SimulationStatus.Cancelled or SimulationStatus.Failed;
    }
}
