using SportsDataService.Application.DTOs;
using SimPitchProtos.SportsDataService;

public static class CountryMapper
{
    public static CountryDto ToDto(this SportsDataService.Domain.Entities.Country country)
    {
        return new CountryDto
        {
            Id = country.Id,
            Name = country.Name,
            Code = country.Code,
            CreatedAt = country.CreatedAt,
            UpdatedAt = country.UpdatedAt
        };
    }

    public static CountryDto ToDto(this CountryGrpc country)
    {
        return new CountryDto
        {
            Id = Guid.Parse(country.Id),
            Name = country.Name,
            Code = country.Code,
            CreatedAt = DateTime.Parse(country.CreatedAt),
            UpdatedAt = DateTime.Parse(country.UpdatedAt)
        };
    }
    public static CountryGrpc ToProto(this CountryDto country)
    {
        return new CountryGrpc
        {
            Id = country.Id.ToString(),
            Name = country.Name,
            Code = country.Code,
            CreatedAt = country.CreatedAt.ToString("o"),
            UpdatedAt = country.UpdatedAt.ToString("o")
        };
    }
}