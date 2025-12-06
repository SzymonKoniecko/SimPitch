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
    private readonly ICountryReadRepository _countryRepository;
    private readonly ICompetitionMembershipReadRepository _competitionMembershipReadRepository;
    private readonly IStadiumReadRepository _stadiumRepository;

    public GetAllTeamsHandler(
        ITeamReadRepository repository,
        ICountryReadRepository countryRepository,
        ICompetitionMembershipReadRepository competitionMembershipReadRepository,
        IStadiumReadRepository stadiumRepository)
    {
        _repository = repository;
        _countryRepository = countryRepository;
        _competitionMembershipReadRepository = competitionMembershipReadRepository;
        _stadiumRepository = stadiumRepository;
    }

    public async Task<IEnumerable<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        List<TeamDto> teamDtos = new List<TeamDto>();

        var teams = await _repository.GetAllTeamsAsync(cancellationToken);
        if (teams == null || !teams.Any())
            throw new KeyNotFoundException("No teams found.");

        foreach (var item in teams)
        {
            var country = await _countryRepository.GetCountryByIdAsync(item.CountryId, cancellationToken);
            if (country is null)
                throw new KeyNotFoundException($"Country with Id '{item.CountryId}' was not found.");

            var stadium = await _stadiumRepository.GetStadiumByIdAsync(item.StadiumId, cancellationToken);
            if (stadium is null)
                throw new KeyNotFoundException($"Stadium with Id '{item.StadiumId}' was not found.");

            var competitionMemberships = await _competitionMembershipReadRepository.GetAllByTeamIdAsync(item.Id, cancellationToken);
            if (competitionMemberships is null)
                throw new KeyNotFoundException($"Competition Memberships with TeamId '{item.Id}' was not found.");

            teamDtos.Add(TeamMapper.ToDto(item, country, stadium, competitionMemberships));
        }

        return teamDtos;
    }
}