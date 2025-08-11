using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Application.Mappers;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
namespace SportsDataService.Application.Features.Teams.Queries.GetAllTeams;

public class GetAllTeamsHandler : IRequestHandler<GetAllTeamsQuery, IEnumerable<TeamDto>>
{
    private readonly ITeamReadRepository _repository;
    private readonly ICountryReadRepository countryRepository;
    private readonly ILeagueReadRepository leagueRepository;
    private readonly IStadiumReadRepository stadiumRepository;

    public GetAllTeamsHandler(
        ITeamReadRepository repository,
        ICountryReadRepository countryRepository,
        ILeagueReadRepository leagueRepository,
        IStadiumReadRepository stadiumRepository)
    {
        _repository = repository;
        this.countryRepository = countryRepository;
        this.leagueRepository = leagueRepository;
        this.stadiumRepository = stadiumRepository;
    }

    public async Task<IEnumerable<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        List<TeamDto> teamDtos = new List<TeamDto>();

        var teams = await _repository.GetAllTeamsAsync(cancellationToken);
        if (teams == null || !teams.Any())
            throw new KeyNotFoundException("No teams found.");

        foreach (var item in teams)
        {
            var country = await countryRepository.GetCountryByIdAsync(item.CountryId, cancellationToken);
            if (country is null)
                throw new KeyNotFoundException($"Country with Id '{item.CountryId}' was not found.");

            var stadium = await stadiumRepository.GetStadiumByIdAsync(item.StadiumId, cancellationToken);
            if (stadium is null)
                throw new KeyNotFoundException($"Stadium with Id '{item.StadiumId}' was not found.");

            var league = await leagueRepository.GetLeagueByIdAsync(item.LeagueId, cancellationToken);
            if (league is null)
                throw new KeyNotFoundException($"League with Id '{item.LeagueId}' was not found.");

            teamDtos.Add(TeamMapper.ToDto(item, country, stadium, league));
        }

        return teamDtos;
    }
}