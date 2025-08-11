using System;
using MediatR;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Domain.Interfaces.Write;

namespace SportsDataService.Application.Features.Stadium.Commands.CreateStadium;

public class CreateStadiumCommandHandler : IRequestHandler<CreateStadiumCommand, Guid>
{
    private readonly IStadiumWriteRepository stadiumRepository;
    private readonly IStadiumReadRepository stadiumReadRepository;

    public CreateStadiumCommandHandler(
        IStadiumWriteRepository stadiumRepository,
        IStadiumReadRepository stadiumReadRepository)
    {
        this.stadiumRepository = stadiumRepository ?? throw new ArgumentNullException(nameof(stadiumRepository));
        this.stadiumReadRepository = stadiumReadRepository;
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

        await stadiumRepository.CreateStadiumAsync(stadium, cancellationToken);
        
        bool exists = await stadiumReadRepository.StadiumExistsAsync(stadium.Id, cancellationToken);
        if (!exists)
            throw new Exception($"Failed to create stadium with Id '{stadium.Id}'.");
        else
            return stadium.Id;
    }
}