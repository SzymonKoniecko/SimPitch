using System;

namespace EngineService.Application.Common.Pagination;


public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}