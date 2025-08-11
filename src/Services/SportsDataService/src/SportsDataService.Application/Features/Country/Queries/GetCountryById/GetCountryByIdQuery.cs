using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.Country.Queries.GetCountryById;

public record GetCountryByIdQuery(Guid CountryId) : IRequest<CountryDto>;
