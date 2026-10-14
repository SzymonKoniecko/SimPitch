using Newtonsoft.Json;
using SimulationService.Domain.Enums;

namespace SimulationService.Application.Mappers;

/// <summary>
/// Writes a season as "2026/2027" - the format EngineService and StatisticsService expect in the JSON
/// they receive (their SeasonYear is a string). Without it Newtonsoft wrote the enum ordinal ("4").
/// Reads the season string, the enum name and the old numeric form, so already stored JSON stays readable.
/// </summary>
public sealed class SeasonYearJsonConverter : JsonConverter<SeasonEnum>
{
    public override void WriteJson(JsonWriter writer, SeasonEnum value, JsonSerializer serializer)
        => writer.WriteValue(value.SeasonEnumToString());

    public override SeasonEnum ReadJson(JsonReader reader, Type objectType, SeasonEnum existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.TokenType switch
        {
            JsonToken.Integer => (SeasonEnum)Convert.ToInt32(reader.Value),
            JsonToken.String when Enum.TryParse<SeasonEnum>((string)reader.Value!, out var season) => season,
            JsonToken.String => ((string)reader.Value!).StringtoSeasonEnum(),
            _ => throw new JsonSerializationException($"Unexpected JSON token {reader.TokenType} for {nameof(SeasonEnum)}.")
        };
    }
}
