using System;
using SimPitchProtos.SimulationService;
using SimulationService.Application.Common.Pagination;
using SimulationService.Application.Mappers;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.API.Helpers;

public static class ProtoHelper
{
    public static PagedRequestDto ValidateProtoAndMapToDto(PagedRequestGrpc pagedRequest)
    {
        if (pagedRequest == null)
        {
            pagedRequest = new();
            pagedRequest.Offset = 0;
            pagedRequest.Limit = 10;
        }
        if (pagedRequest.SortingMethod == null)
        {
            pagedRequest.SortingMethod = new();
            pagedRequest.SortingMethod.SortingOption = "CreatedDate";
            pagedRequest.SortingMethod.Order = "DESC";
        }

        return new PagedRequestDto
        (
            pagedRequest.Offset,
            pagedRequest.Limit,
            pagedRequest.SortingMethod.SortingOption,
            pagedRequest.SortingMethod.Order
        );
    }

    public static PagedResponseGrpc MapPagedResultsToProto(PagedResponseDetails details)
    {
        return new SimPitchProtos.SimulationService.PagedResponseGrpc
        {
            TotalCount = details.TotalCount,
            SortingOption = details.SortingOption,
            SortingOrder = details.Order
        };
    }
}
