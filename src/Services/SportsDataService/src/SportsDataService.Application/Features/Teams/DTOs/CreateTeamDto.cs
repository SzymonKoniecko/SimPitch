using SportsDataService.Domain.Enums;

namespace SportsDataService.Application.Teams.DTOs
{
    public class CreateTeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
        public Guid StadiumId { get; set; } = Guid.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}