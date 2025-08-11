using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Write;

public interface IStadiumWriteRepository
{
    Task CreateStadiumAsync(Stadium stadium, CancellationToken cancellationToken);
}
