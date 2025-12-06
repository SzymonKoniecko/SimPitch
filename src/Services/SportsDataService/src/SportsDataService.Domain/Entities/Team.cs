using SportsDataService.Domain.Enums;

namespace SportsDataService.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public Guid StadiumId { get; set; } = Guid.Empty;
    public string ShortName { get; set; } = string.Empty;
}