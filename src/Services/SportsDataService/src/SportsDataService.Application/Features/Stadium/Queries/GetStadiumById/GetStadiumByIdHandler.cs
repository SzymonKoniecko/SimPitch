using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.Stadium.Queries.GetStadiumById;

public class GetStadiumByIdHandler : IRequestHandler<GetStadiumByIdQuery, StadiumDto>
{
    private readonly IStadiumReadRepository _stadiumRepository;

    public GetStadiumByIdHandler(IStadiumReadRepository stadiumRepository)
    {
        _stadiumRepository = stadiumRepository;
    }

    public async Task<StadiumDto> Handle(GetStadiumByIdQuery request, CancellationToken cancellationToken)
    {
        var stadium = await _stadiumRepository.GetStadiumByIdAsync(request.stadiumId, cancellationToken);
        if (stadium is null)
        {
            throw new KeyNotFoundException($"Stadium with Id '{request.stadiumId}' was not found.");
        }
        return StadiumMapper.ToDto(stadium);
    }
}
