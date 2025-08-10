using System;
using MediatR;
using SportsDataService.Domain.Interfaces.Write;

namespace SportsDataService.Application.Features.Stadium.Commands.CreateStadium;

public class CreateStadiumCommandHandler : IRequestHandler<CreateStadiumCommand, Guid>
{
    private readonly IStadiumWriteRepository _stadiumRepository;

    public CreateStadiumCommandHandler(IStadiumWriteRepository stadiumRepository)
    {
        _stadiumRepository = stadiumRepository ?? throw new ArgumentNullException(nameof(stadiumRepository));
    }

    public async Task<Guid> Handle(CreateStadiumCommand request, CancellationToken cancellationToken)
    {
        var stadium = new SportsDataService.Domain.Entities.Stadium
        {
            Id = Guid.NewGuid(),
            Name = request.Stadium.Name,
            Capacity = request.Stadium.Capacity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var stadiumId = await _stadiumRepository.CreateStadiumAsync(stadium, cancellationToken);
        return stadiumId;
    }
}