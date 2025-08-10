using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.Stadium.Queries.GetStadiumById;

public class GetStadiumByIdHandler
{
    private readonly IStadiumReadRepository _stadiumRepository;

    public GetStadiumByIdHandler(IStadiumReadRepository stadiumRepository)
    {
        _stadiumRepository = stadiumRepository;
    }

    public async Task<StadiumDto> Handle(GetStadiumByIdQuery request, CancellationToken cancellationToken)
    {
        var stadium = await _stadiumRepository.GetStadiumByIdAsync(request.StadiumId, cancellationToken);
        if (stadium is null)
        {
            throw new KeyNotFoundException($"Stadium with Id '{request.StadiumId}' was not found.");
        }
        return StadiumMapper.ToDto(stadium);
    }
}
