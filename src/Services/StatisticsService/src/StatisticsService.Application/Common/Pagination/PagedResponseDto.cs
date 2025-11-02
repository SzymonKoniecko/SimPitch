using System;
using StatisticsService.Application.Common.Sorting;
namespace StatisticsService.Application.Common.Pagination;


public class PagedRequestDto<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public SortingMethodDto SortingMethod { get; set; }
}