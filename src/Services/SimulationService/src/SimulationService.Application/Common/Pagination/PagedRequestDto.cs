using System;
using SimulationService.Application.Common.Sorting;

namespace SimulationService.Application.Common.Pagination;


public class PagedRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingMethodDto SortingMethod { get; set; }

    public PagedRequestDto(int pageNumber, int pageSize, SortingOptionEnum sortingOptionEnum, string condition)
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