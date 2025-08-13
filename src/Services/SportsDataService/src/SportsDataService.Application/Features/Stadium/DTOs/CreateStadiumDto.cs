using System;

namespace SportsDataService.Application.Stadiums.DTOs;

public class CreateStadiumDto
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
