using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;
using SimPitchProtos.SportsDataService;
public static class LeagueMapper
{
    public static LeagueDto ToDto(this League league)
    {
        return new LeagueDto
        {
            Id = league.Id,
            Name = league.Name,
            CountryId = league.CountryId,
            Sport = league.Sport.ToString(),
            CreatedAt = league.CreatedAt,
            UpdatedAt = league.UpdatedAt
        };
    }
    public static LeagueDto ToDto(this LeagueGrpc league)
    {
        return new LeagueDto
        {
            Id = Guid.Parse(league.Id),
            Name = league.Name,
            Sport = league.Sport,
            CountryId = Guid.Parse(league.CountryId),
            CreatedAt = DateTime.Parse(league.CreatedAt),
            UpdatedAt = DateTime.Parse(league.UpdatedAt)
        };
    }
    public static LeagueGrpc ToProto(this LeagueDto league)
    {
        return new LeagueGrpc
        {
            Id = league.Id.ToString(),
            Name = league.Name,
            Sport = league.Sport,
            CountryId = league.CountryId.ToString(),
            CreatedAt = league.CreatedAt.ToString("o"),
            UpdatedAt = league.UpdatedAt.ToString("o")
        };
    }
}