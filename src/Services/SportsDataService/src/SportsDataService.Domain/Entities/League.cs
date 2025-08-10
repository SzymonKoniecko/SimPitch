using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Domain.Entities;

public class League
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public SportEnum Sport { get; set; }
    public Guid CountryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
