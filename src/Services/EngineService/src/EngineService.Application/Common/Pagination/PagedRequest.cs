using System;
using EngineService.Application.Common.Sorting;

namespace EngineService.Application.Common.Pagination;


public class PagedRequest
{
    public int Offset { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingMethod SortingMethod { get; set; }

    public PagedRequest(int offset, int pageSize, string sortingOptionEnum, string order)
    {

        this.Offset = offset;
        this.PageSize = pageSize;
        SortingMethod = new()
        {
            SortingOption = sortingOptionEnum,
            Order = order
        };
    }
}