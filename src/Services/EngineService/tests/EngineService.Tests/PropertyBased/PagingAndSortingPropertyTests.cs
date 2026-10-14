using EngineService.Application.Common.Pagination;
using EngineService.Application.Mappers;
using EngineService.Domain.Enums;
using EngineService.Domain.ValueObjects;
using FsCheck;
using FsCheck.Xunit;

namespace EngineService.Tests.PropertyBased;

public class PagingAndSortingPropertyTests
{
    private static readonly SortingOptionEnum[] SortingOptions = Enum.GetValues<SortingOptionEnum>();

    [Property]
    public bool SortingOption_RoundTrips_ForEveryEnumValue(NonNegativeInt index)
    {
        var option = SortingOptions[index.Get % SortingOptions.Length];

        return EnumMapper.SortingOptionToEnum(option.ToString()) == option;
    }

    [Property]
    public bool SortingOption_RejectsUnknownValues(string value)
    {
        if (Enum.GetNames<SortingOptionEnum>().Contains(value))
            return true; // generated value happens to be a valid name

        return Record.Exception(() => EnumMapper.SortingOptionToEnum(value)) is KeyNotFoundException;
    }

    /// <summary>TotalPages is the smallest number of pages that fits all items.</summary>
    [Property]
    public bool TotalPages_IsSmallestNumberOfPagesThatFitsAllItems(NonNegativeInt totalCount, PositiveInt pageSize)
    {
        var response = new PagedResponse<int> { TotalCount = totalCount.Get, PageSize = pageSize.Get };

        int pages = response.TotalPages;
        return (long)pages * pageSize.Get >= totalCount.Get
            && (totalCount.Get == 0 ? pages == 0 : (long)(pages - 1) * pageSize.Get < totalCount.Get);
    }

    [Property]
    public bool PagedResponseMapper_CopiesItemsAndDetails(int seed, NonNegativeInt totalCount, PositiveInt pageNumber, PositiveInt pageSize, NonNegativeInt sortingIndex, bool descending)
    {
        var f = EngineFakers.Create(seed);
        var items = Enumerable.Range(0, f.Random.Int(0, 20)).Select(_ => f.Random.Guid()).ToList();
        var details = new PagedResponseDetails
        {
            TotalCount = totalCount.Get,
            PageNumber = pageNumber.Get,
            PageSize = pageSize.Get,
            SortingOption = SortingOptions[sortingIndex.Get % SortingOptions.Length].ToString(),
            Order = descending ? "DESC" : "ASC"
        };

        var response = PagedResponseMapper<Guid>.ToPagedResponse(items, details);

        return response.Items.SequenceEqual(items)
            && response.TotalCount == details.TotalCount
            && response.PageNumber == details.PageNumber
            && response.PageSize == details.PageSize
            && response.SortingMethod.SortingOption == details.SortingOption
            && response.SortingMethod.Order == details.Order;
    }
}
