using Newtonsoft.Json;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Domain.Enums;

namespace SimulationService.Tests.ClassTests;

public class SeasonYearJsonConverterTests
{
    [Fact]
    public void SeasonYear_IsWrittenAsSeasonString()
    {
        var json = JsonConvert.SerializeObject(new SeasonStatsDto { SeasonYear = SeasonEnum.Season2026_2027 });

        Assert.Contains("\"SeasonYear\":\"2026/2027\"", json);
    }

    [Theory]
    [InlineData("4")]                  // numeric form stored in SimulationDb before the converter existed
    [InlineData("\"2026/2027\"")]      // current wire format
    [InlineData("\"Season2026_2027\"")] // enum name
    public void SeasonYear_IsReadFromAllKnownForms(string jsonValue)
    {
        var dto = JsonConvert.DeserializeObject<SeasonStatsDto>($"{{\"SeasonYear\":{jsonValue}}}");

        Assert.Equal(SeasonEnum.Season2026_2027, dto!.SeasonYear);
    }
}
