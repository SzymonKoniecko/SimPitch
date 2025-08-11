using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.Country.Queries.GetCountryById;

public class GetCountryByIdHandler
{
    private readonly ICountryReadRepository _countryRepository;
    public GetCountryByIdHandler(ICountryReadRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }
    public async Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetCountryByIdAsync(request.CountryId, cancellationToken);
        if (country is null)
        {
            throw new KeyNotFoundException($"Country with Id '{request.CountryId}' was not found.");
        }
        return CountryMapper.ToDto(country);
    }
}
