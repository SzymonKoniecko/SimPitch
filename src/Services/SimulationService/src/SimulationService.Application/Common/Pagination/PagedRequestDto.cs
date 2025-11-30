using System;
using SimulationService.Application.Common.Sorting;

namespace SimulationService.Application.Common.Pagination;


public class PagedRequestDto
{
    public int Offset { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingMethodDto SortingMethod { get; set; }

    public PagedRequestDto(int offset, int pageSize, string sortingOption, string condition, string order)
    {

        this.Offset = offset;
        this.PageSize = pageSize;
        SortingMethod = new()
        {
            SortingOption = sortingOption,
            Condition = condition,
            Order = order
        };
    }
}