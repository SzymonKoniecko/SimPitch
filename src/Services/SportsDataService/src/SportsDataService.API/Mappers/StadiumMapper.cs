using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;
using SimPitchProtos.SportsDataService.Team;
using SimPitchProtos.SportsDataService.Stadium;
using SimPitchProtos.SportsDataService;
using SportsDataService.Application.Stadiums.DTOs;
public static class StadiumMapper
{
    public static StadiumDto ToDto(this Stadium stadium)
    {
        return new StadiumDto
        {
            Id = stadium.Id,
            Name = stadium.Name,
            Capacity = stadium.Capacity,
            CreatedAt = stadium.CreatedAt,
            UpdatedAt = stadium.UpdatedAt
        };
    }
    public static StadiumDto ToDto(this StadiumGrpc stadium)
    {
        return new StadiumDto
        {
            Id = Guid.Parse(stadium.Id),
            Name = stadium.Name,
            Capacity = stadium.Capacity,
            CreatedAt = DateTime.Parse(stadium.CreatedAt),
            UpdatedAt = DateTime.Parse(stadium.UpdatedAt)
        };
    }

    internal static StadiumDto ToDto(this CreateStadiumDto s)
    {
        return new StadiumDto
        {
            Name = s.Name,
            Capacity = s.Capacity
        };
    }

    public static StadiumGrpc ToProto(this StadiumDto stadium)
    {
        return new StadiumGrpc
        {
            Id = stadium.Id.ToString(),
            Name = stadium.Name,
            Capacity = stadium.Capacity,
            CreatedAt = stadium.CreatedAt.ToString("o"),
            UpdatedAt = stadium.UpdatedAt.ToString("o")
        };
    }

    public static CreateStadiumDto ToDto(this CreateStadiumRequest request)
    {
        return new CreateStadiumDto
        {
            Name = request.Name,
            Capacity = request.Capacity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}