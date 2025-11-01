using System;
using StatisticsService.Application.Common.Pagination;
using StatisticsService.Application.Consts;
using StatisticsService.Application.DTOs;
namespace StatisticsService.Application.Interfaces;

public interface IIterationResultGrpcClient
{
    Task<List<IterationResultDto>> GetPagedIterationResultsBySimulationIdAsync(
        Guid simulationId,
        PagedRequestDto pagedRequest,
        CancellationToken cancellationToken);
    Task<List<IterationResultDto>> GetAllIterationResultsBySimulationIdAsync(
        Guid simulationId,
        int pageSize = Pagination.PAGINATION_PAGE_LIMIT,
        CancellationToken cancellationToken = default);
}
