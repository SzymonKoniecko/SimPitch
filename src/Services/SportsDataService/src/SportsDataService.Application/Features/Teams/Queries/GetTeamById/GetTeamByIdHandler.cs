using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Application.Mappers;

namespace SportsDataService.Application.Features.Teams.Queries.GetTeamById;
public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
{
    private readonly ITeamReadRepository _repository;
    private readonly ICountryReadRepository countryRepository;
    private readonly ILeagueReadRepository leagueRepository;
    private readonly IStadiumReadRepository stadiumRepository;

    public GetTeamByIdHandler(
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

    public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _repository.GetTeamByIdAsync(request.TeamId, cancellationToken);
        if (team is null)
            throw new KeyNotFoundException($"Team with Id '{request.TeamId}' was not found.");

        var country = await countryRepository.GetCountryByIdAsync(team.CountryId, cancellationToken);
        if (country is null)
            throw new KeyNotFoundException($"Country with Id '{team.CountryId}' was not found.");

        var stadium = await stadiumRepository.GetStadiumByIdAsync(team.StadiumId, cancellationToken);
        if (stadium is null)
            throw new KeyNotFoundException($"Stadium with Id '{team.StadiumId}' was not found.");

        var league = await leagueRepository.GetLeagueByIdAsync(team.LeagueId, cancellationToken);
        if (league is null)
            throw new KeyNotFoundException($"League with Id '{team.LeagueId}' was not found.");
        
        return TeamMapper.ToDto(team, country, stadium, league);
    }
}
