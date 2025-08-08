using MediatR;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, Team>
{
    private readonly ITeamReadRepository _repository;

    public GetTeamByIdHandler(ITeamReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Team> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _repository.GetTeamByIdAsync(request.TeamId);
        if (team is null)
        {
            throw new KeyNotFoundException($"Team with Id '{request.TeamId}' was not found.");
        }
        return team;
    }
}
