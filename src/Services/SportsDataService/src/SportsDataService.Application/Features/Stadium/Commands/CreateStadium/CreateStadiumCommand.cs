using System;
using MediatR;
using SportsDataService.Application.Stadiums.DTOs;

namespace SportsDataService.Application.Features.Stadium.Commands.CreateStadium;

public record CreateStadiumCommand(CreateStadiumDto Stadium) : IRequest<Guid>;