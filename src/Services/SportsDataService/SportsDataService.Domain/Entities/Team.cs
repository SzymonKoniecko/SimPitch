namespace SportsDataService.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public Guid StadiumId { get; set; }
    public Guid LeagueId { get; set; }
    public string LogoUrl { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
}