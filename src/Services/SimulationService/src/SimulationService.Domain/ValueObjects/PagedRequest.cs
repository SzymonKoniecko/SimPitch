using System;
using SimulationService.Domain.Enums;
using StackExchange.Redis;

namespace SimulationService.Domain.ValueObjects;

public class PagedRequest
{
    public int Offset { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingMethod SortingMethod { get; set; }

    public PagedRequest(int offset, int pageSize, SortingOptionEnum sortingOptionEnum, string condition, string order)
    {

        this.Offset = offset;
        this.PageSize = pageSize;
        SortingMethod = new()
        {
            SortingOption = sortingOptionEnum,
            Condition = condition,
            Order = order
        };
    }
} 