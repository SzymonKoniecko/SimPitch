using System;
using EngineService.Application.Common.Sorting;

namespace EngineService.Application.Common.Pagination;


public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingMethod SortingMethod { get; set; }

    public PagedRequest(int pageNumber, int pageSize, SortingOptionEnum sortingOptionEnum, string condition)
    {

        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        SortingMethod = new()
        {
            SortingOption = sortingOptionEnum,
            Condition = condition
        };
    }
}