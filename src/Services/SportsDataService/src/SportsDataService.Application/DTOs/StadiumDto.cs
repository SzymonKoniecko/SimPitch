using System;

namespace SportsDataService.Application.DTOs;

public class StadiumDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; } = 0;
}
