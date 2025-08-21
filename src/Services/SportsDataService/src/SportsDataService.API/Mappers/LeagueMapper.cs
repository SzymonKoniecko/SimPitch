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
            MaxRound = league.MaxRound,
            Strength = league.Strength
        };
    }
    public static LeagueDto ToDto(this LeagueGrpc league)
    {
        return new LeagueDto
        {
            Id = Guid.Parse(league.Id),
            Name = league.Name,
            MaxRound = (int)league.MaxRound,
            CountryId = Guid.Parse(league.CountryId),
            Strength = league.Strength
        };
    }
    public static LeagueGrpc ToProto(this LeagueDto league)
    {
        return new LeagueGrpc
        {
            Id = league.Id.ToString(),
            Name = league.Name,
            MaxRound = league.MaxRound,
            CountryId = league.CountryId.ToString(),
            Strength = league.Strength
        };
    }
}