using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Application.Mappers;

namespace SportsDataService.Application.Features.Teams.Queries.GetTeamById;
public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
{
    private readonly ITeamReadRepository _repository;
    private readonly ICountryReadRepository _countryRepository;
    private readonly ICompetitionMembershipReadRepository _competitionMembershipReadRepository;
    private readonly IStadiumReadRepository _stadiumRepository;

    public GetTeamByIdHandler(
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

    public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _repository.GetTeamByIdAsync(request.TeamId, cancellationToken);
        if (team is null)
            throw new KeyNotFoundException($"Team with Id '{request.TeamId}' was not found.");

        var country = await _countryRepository.GetCountryByIdAsync(team.CountryId, cancellationToken);
        if (country is null)
            throw new KeyNotFoundException($"Country with Id '{team.CountryId}' was not found.");

        var stadium = await _stadiumRepository.GetStadiumByIdAsync(team.StadiumId, cancellationToken);
        if (stadium is null)
            throw new KeyNotFoundException($"Stadium with Id '{team.StadiumId}' was not found.");

        var competitionMemberships = await _competitionMembershipReadRepository.GetAllByTeamIdAsync(team.Id, cancellationToken);
        if (competitionMemberships is null)
            throw new KeyNotFoundException($"Competition Memberships with TeamId '{team.Id}' was not found.");
        
        return TeamMapper.ToDto(team, country, stadium, competitionMemberships);
    }
}
