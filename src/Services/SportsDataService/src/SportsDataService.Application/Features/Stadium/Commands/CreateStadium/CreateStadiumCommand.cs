using System;
using MediatR;
using SportsDataService.Application.DTOs.Feature;

namespace SportsDataService.Application.Features.Stadium.Commands.CreateStadium;

public record CreateStadiumCommand(CreateStadiumDto Stadium) : IRequest<Guid>;