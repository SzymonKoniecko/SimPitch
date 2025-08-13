using SportsDataService.Domain.Enums;

namespace SportsDataService.Application.DTOs.Feature
{
    public class CreateTeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
        public Guid StadiumId { get; set; } = Guid.Empty;
        public Guid LeagueId { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}