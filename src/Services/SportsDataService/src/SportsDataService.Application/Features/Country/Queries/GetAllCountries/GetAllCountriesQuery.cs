using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.Country.Queries.GetAllCountries;

public record GetAllCountriesQuery : IRequest<IEnumerable<CountryDto>>;