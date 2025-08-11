using MediatR;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Domain.Interfaces.Write;
using SportsDataService.Application.DTOs.Feature;
using SportsDataService.Application.Mappers;
namespace SportsDataService.Application.Features.Teams.Commands.CreateTeam;
public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Guid>
{
    private readonly ITeamWriteRepository teamWriteRepository;
    private readonly ICountryReadRepository countryReadRepository;
    private readonly ILeagueReadRepository leagueReadRepository;
    private readonly IStadiumReadRepository stadiumReadRepository;
    private readonly ITeamReadRepository teamReadRepository;

    public CreateTeamCommandHandler(
        ITeamWriteRepository teamWriteRepository,
        ITeamReadRepository teamReadRepository,
        ICountryReadRepository countryReadRepository,
        ILeagueReadRepository leagueReadRepository,
        IStadiumReadRepository stadiumReadRepository)
    {
        this.teamWriteRepository = teamWriteRepository;
        this.countryReadRepository = countryReadRepository;
        this.leagueReadRepository = leagueReadRepository;
        this.stadiumReadRepository = stadiumReadRepository;
        this.teamReadRepository = teamReadRepository;
    }

    public async Task<Guid> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var teamEntity = TeamMapper.ToDomain(request.Team);
        
        teamEntity.Id = Guid.NewGuid();

        await teamWriteRepository.CreateTeamAsync(teamEntity, cancellationToken);

        bool exists = await teamReadRepository.TeamExistsAsync(teamEntity.Id, cancellationToken);
        if (!exists)
            throw new Exception($"Failed to create team with Id '{teamEntity.Id}'.");
        else
            return teamEntity.Id;
    }
}