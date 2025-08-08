using MediatR;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Read;

public class GetAllTeamsHandler : IRequestHandler<GetAllTeamsQuery, IEnumerable<Team>>
{
    private readonly ITeamReadRepository _repository;

    public GetAllTeamsHandler(ITeamReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Team>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllTeamsAsync();
    }
}