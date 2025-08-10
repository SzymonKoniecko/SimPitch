using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.Stadium.Queries.GetAllStadiums;

public class GetAllStadiumsHandler : IRequestHandler<GetAllStadiumsQuery, IEnumerable<StadiumDto>>
{

    private readonly IStadiumReadRepository _stadiumRepository;

    public GetAllStadiumsHandler(IStadiumReadRepository stadiumRepository)
    {
        _stadiumRepository = stadiumRepository;
    }

    public async Task<IEnumerable<StadiumDto>> Handle(GetAllStadiumsQuery request, CancellationToken cancellationToken)
    {
        var stadiums = await _stadiumRepository.GetAllStadiumsAsync(cancellationToken);
        return stadiums.Select(s => StadiumMapper.ToDto(s));
    }
}
