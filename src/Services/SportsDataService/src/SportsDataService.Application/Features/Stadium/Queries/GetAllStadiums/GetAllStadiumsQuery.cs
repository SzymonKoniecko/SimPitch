using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.Stadium.Queries.GetAllStadiums;

public record GetAllStadiumsQuery : IRequest<IEnumerable<StadiumDto>>;