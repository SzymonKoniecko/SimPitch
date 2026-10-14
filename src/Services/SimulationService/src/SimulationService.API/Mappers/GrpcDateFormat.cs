namespace SimulationService.API.Mappers;

/// <summary>
/// Date format of the gRPC contract (parsed by EngineService with ParseExact + InvariantCulture).
/// Must not depend on the server culture - "dd/MM/yyyy" machines broke the parsing.
/// </summary>
public static class GrpcDateFormat
{
    public const string Pattern = "MM/dd/yyyy HH:mm:ss";
}
