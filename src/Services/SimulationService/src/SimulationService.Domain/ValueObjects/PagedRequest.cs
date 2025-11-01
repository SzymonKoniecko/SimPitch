using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingMethod SortingMethod { get; set; }

    public PagedRequest(int pageNumber, int pageSize, string sortingOptionEnum, string condition)
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