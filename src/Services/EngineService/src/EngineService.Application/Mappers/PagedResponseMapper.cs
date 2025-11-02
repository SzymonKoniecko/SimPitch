using System;
using EngineService.Application.Common.Pagination;
using EngineService.Domain.ValueObjects;

namespace EngineService.Application.Mappers;

public static class PagedResponseMapper<T>
{
    public static PagedResponse<T> ToPagedResponse(IEnumerable<T> itemsList, PagedResponseDetails pagedResponseDetails)
    {
        var response = new PagedResponse<T>();
        response.Items = itemsList;
        response.PageNumber = pagedResponseDetails.PageNumber;
        response.PageSize = pagedResponseDetails.PageSize;
        response.TotalCount = pagedResponseDetails.TotalCount;
        response.SortingMethod = new();
        response.SortingMethod.SortingOption = pagedResponseDetails.SortingOption;
        response.SortingMethod.Order = pagedResponseDetails.Order;

        return response;
    }
}
