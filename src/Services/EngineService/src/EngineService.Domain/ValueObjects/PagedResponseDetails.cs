using System;

namespace EngineService.Domain.ValueObjects;

public class PagedResponseDetails
{
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SortingOption { get; set; }
    public string Condition { get; set; }
}
