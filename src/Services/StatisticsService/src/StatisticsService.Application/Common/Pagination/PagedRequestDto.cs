using System;
using StatisticsService.Application.Common.Sorting;

namespace StatisticsService.Application.Common.Pagination;


public class PagedRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingMethodDto SortingMethod { get; set; } = null;
    public PagedRequestDto(int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
    }

    public PagedRequestDto(int pageNumber, int pageSize, string sortingOptionEnum, string order)
    {

        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        SortingMethod = new()
        {
            SortingOption = sortingOptionEnum,
            Order = order
        };
    }
}