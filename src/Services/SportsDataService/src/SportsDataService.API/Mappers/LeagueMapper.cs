using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;
using SimPitchProtos.SportsDataService;
using SportsDataService.API.Mappers;
public static class LeagueMapper
{
    public static LeagueGrpc ToProto(this LeagueDto league)
    {
        return new LeagueGrpc
        {
            Id = league.Id.ToString(),
            Name = league.Name,
            MaxRound = league.MaxRound,
            CountryId = league.CountryId.ToString(),
            LeagueStrengths = { league.Strengths.Select(x => ToProto(x))}
        };
    }

    public static LeagueStrengthGrpc ToProto(this LeagueStrengthDto dto)
    {
        return new LeagueStrengthGrpc
        {
            Id = dto.Id.ToString(),
            LeagueId = dto.LeagueId.ToString(),
            SeasonYear = EnumMapper.SeasonEnumToString(dto.SeasonYear),
            Strength = dto.Strength
        };
    }
}