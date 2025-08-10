using MediatR;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Domain.Interfaces.Write;
using SportsDataService.Application.DTOs.Feature;
using SportsDataService.Application.Mappers;
namespace SportsDataService.Application.Features.Teams.Commands.CreateTeam;
public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Guid>
{
    private readonly ITeamWriteRepository _teamRepository;
    private readonly ICountryReadRepository countryReadRepository;
    private readonly ILeagueReadRepository leagueReadRepository;
    private readonly IStadiumReadRepository stadiumReadRepository;

    public CreateTeamCommandHandler(
        ITeamWriteRepository teamRepository,
        ICountryReadRepository countryReadRepository,
        ILeagueReadRepository leagueReadRepository,
        IStadiumReadRepository stadiumReadRepository)
    {
        _teamRepository = teamRepository;
        this.countryReadRepository = countryReadRepository;
        this.leagueReadRepository = leagueReadRepository;
        this.stadiumReadRepository = stadiumReadRepository;
    }

    public async Task<Guid> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var teamEntity = TeamMapper.ToDomain(request.Team);
        
        teamEntity.Id = Guid.NewGuid();

        var createdTeamId = await _teamRepository.CreateTeamAsync(teamEntity, cancellationToken);
        return createdTeamId;
    }
}